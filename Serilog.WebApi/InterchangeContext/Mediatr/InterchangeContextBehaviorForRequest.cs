using MediatR;
using Serilog.WebApi.InterchangeContext.Dto;
using Serilog.WebApi.InterchangeContext.Services;
using System.Diagnostics;
using System.Reflection;

namespace Serilog.WebApi.InterchangeContext.Mediatr;

public class InterchangeContextBehaviorForRequest<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IInterchangeContext _interchangeContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InterchangeContextBehaviorForRequest(IInterchangeContext interchangeContext, IHttpContextAccessor httpContextAccessor)
    {
        _interchangeContext = interchangeContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        ArgumentNullException.ThrowIfNull(httpContext, "HttpContext");
        if (!_interchangeContext.IsCreated)
        {
            string interchangeId = GetInterchangeId(httpContext);
            await _interchangeContext.Create(interchangeId, typeof(TRequest).FullName ?? string.Empty, cancellationToken);
        }
        await PopulateContextProperties(httpContext, request, cancellationToken);

        return await next();
    }

    private string GetInterchangeId(HttpContext context)
    {
        return Activity.Current?.Id ?? context.TraceIdentifier;
    }

    private async Task PopulateContextProperties(HttpContext httpContext, TRequest instance, CancellationToken cancellationToken)
    {
        if (instance is null)
            return;
        var instanceType = instance.GetType();
        var populatorType = typeof(IInterchangeContextPropertyPopulator<>).MakeGenericType(instanceType);
        if (populatorType is null)
        {
            return;
        }
        var populator = httpContext.RequestServices.GetService(populatorType);
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
            await _interchangeContext.SetProperty(property, cancellationToken);
        }
    }
}