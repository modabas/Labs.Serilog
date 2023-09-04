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
        var publisherType = typeof(IContextPublisher<>).MakeGenericType(instanceType);
        if (publisherType is null)
        {
            return;
        }
        var publisher = httpContext.RequestServices.GetService(publisherType);
        if (publisher is null)
        {
            return;
        }

        var createPromotionsTask = (Task?)publisherType
            .GetTypeInfo()
            .GetMethod("CreatePromotions")?
            .Invoke(publisher, new object[] { instance, cancellationToken });
        if (createPromotionsTask is null)
        {
            return;
        }
        await createPromotionsTask;

        var getPromotionsTask = (Task<IEnumerable<ContextProperty>>?)publisherType
            .GetTypeInfo()
            .GetMethod("GetPromotions")?
            .Invoke(publisher, new object[] { instance, cancellationToken });
        if (getPromotionsTask is null)
        {
            return;
        }
        foreach (var promotion in await getPromotionsTask)
        {
            await _interchangeContext.SetProperty(promotion, cancellationToken);
        }
    }
}
