using MediatR;
using Serilog.WebApi.InterchangeContext.Dto;
using Serilog.WebApi.InterchangeContext.Services;
using System.Reflection;

namespace Serilog.WebApi.InterchangeContext.Mediatr;

public class InterchangeContextPopulateRequestPropertiesBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IInterchangeContextAccessor _interchangeContextAccessor;

    public InterchangeContextPopulateRequestPropertiesBehavior(IInterchangeContextAccessor interchangeContextAccessor)
    {
        _interchangeContextAccessor = interchangeContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var interchangeContext = _interchangeContextAccessor.InterchangeContext;
        if (interchangeContext is not null)
        {
            await PopulateRequestProperties(interchangeContext, request, cancellationToken);
        }

        return await next();
    }

    private async Task PopulateRequestProperties(IInterchangeContext interchangeContext, TRequest instance, CancellationToken cancellationToken)
    {
        if (instance is null)
            return;
        var instanceType = instance.GetType();
        var populatorType = typeof(IInterchangeContextPropertyPopulator<>).MakeGenericType(instanceType);
        if (populatorType is null)
        {
            return;
        }
        var populator = interchangeContext.Services.GetService(populatorType);
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
            await interchangeContext.SetProperty(property, cancellationToken);
        }
    }
}