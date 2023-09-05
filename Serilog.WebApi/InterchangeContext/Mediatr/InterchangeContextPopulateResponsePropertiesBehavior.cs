using MediatR;
using Serilog.WebApi.InterchangeContext.Dto;
using Serilog.WebApi.InterchangeContext.Services;
using System.Diagnostics;
using System.Reflection;

namespace Serilog.WebApi.InterchangeContext.Mediatr;

public class InterchangeContextPopulateResponsePropertiesBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IInterchangeContextAccessor _interchangeContextAccessor;

    public InterchangeContextPopulateResponsePropertiesBehavior(IInterchangeContextAccessor interchangeContextAccessor)
    {
        _interchangeContextAccessor = interchangeContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        var interchangeContext = _interchangeContextAccessor.InterchangeContext;
        if (interchangeContext is not null)
        {
            await PopulateResponseProperties(interchangeContext, response, cancellationToken);
        }
        return response;
    }

    private string GetInterchangeId(HttpContext context)
    {
        return Activity.Current?.Id ?? context.TraceIdentifier;
    }

    private async Task PopulateResponseProperties(IInterchangeContext httpContext, TResponse instance, CancellationToken cancellationToken)
    {
        if (instance is null)
            return;
        var instanceType = instance.GetType();
        
        //if no response type for mediator handler (only returns Task)
        if (instanceType == typeof(MediatR.Unit))
            return;

        var populatorType = typeof(IInterchangeContextPropertyPopulator<>).MakeGenericType(instanceType);
        if (populatorType is null)
        {
            return;
        }
        var populator = httpContext.Services.GetService(populatorType);
        if (populator is null)
        {
            return;
        }

        var createPropertiesTask = (Task?)populatorType
            .GetTypeInfo()
            .GetMethod("SetProperties")?
            .Invoke(populator, new object[] { instance, cancellationToken });
        if (createPropertiesTask is null)
        {
            return;
        }
        await createPropertiesTask;

        var getPropertiesTask = (Task<IEnumerable<ContextProperty>>?)populatorType
            .GetTypeInfo()
            .GetMethod("GetProperties")?
            .Invoke(populator, new object[] { instance, cancellationToken });
        if (getPropertiesTask is null)
        {
            return;
        }
        foreach (var property in await getPropertiesTask)
        {
            await httpContext.SetProperty(property, cancellationToken);
        }
    }
}
