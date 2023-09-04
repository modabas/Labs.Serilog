using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Reflection;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.InterchangeContext.Filters;
public class InterchangeContextFilter : IAsyncActionFilter
{

    private readonly IInterchangeContext _interchangeContext;

    public InterchangeContextFilter(IInterchangeContext interchangeContext)
    {
        _interchangeContext = interchangeContext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        var cancellationToken = context.HttpContext.RequestAborted;
        if (!_interchangeContext.IsCreated)
        {
            string interchangeId = GetInterchangeId(context.HttpContext);
            await _interchangeContext.Create(interchangeId, context.ActionDescriptor.DisplayName ?? string.Empty, cancellationToken);
        }
        foreach (var arg in context.ActionArguments)
        {
            await PublishContextProperties(context.HttpContext, arg.Value, cancellationToken);
        }
        await next();

    }

    private string GetInterchangeId(HttpContext context)
    {
        return Activity.Current?.Id ?? context.TraceIdentifier;
    }

    private async Task PublishContextProperties<TRequest>(HttpContext httpContext, TRequest instance, CancellationToken cancellationToken)
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
            .GetMethod("AddProperties")?
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
