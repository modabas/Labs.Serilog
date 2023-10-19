using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.Web.InterchangeContext.Filters;
public class InterchangeContextFactoryFilter : IAsyncActionFilter
{
    private readonly IInterchangeContextAccessor _interchangeContextAccessor;
    private readonly IInterchangeContext _interchangeContext;

    public InterchangeContextFactoryFilter(IInterchangeContextAccessor interchangeContextAccessor, IInterchangeContext interchangeContext)
    {
        _interchangeContextAccessor = interchangeContextAccessor;
        _interchangeContext = interchangeContext;
    }


    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!_interchangeContextAccessor.IsInitialized)
        {
            _interchangeContext.Name = context.ActionDescriptor.DisplayName ?? string.Empty;
            _interchangeContextAccessor.InterchangeContext = _interchangeContext;
        }

        await next();
    }
}