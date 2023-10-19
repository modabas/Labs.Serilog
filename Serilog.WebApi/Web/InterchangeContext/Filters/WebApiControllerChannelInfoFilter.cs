using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.Serilog.DataExchangeLogger.Loggers;
using Serilog.WebApi.Web.InterchangeContext.Dto;
using Serilog.WebApi.Web.InterchangeContext.Extensions;
using System.Net;
using System.Text.RegularExpressions;

namespace Serilog.WebApi.Web.InterchangeContext.Filters;

public class WebApiControllerChannelInfoFilter : IAsyncActionFilter
{
    private readonly IDataExchangeLogger<WebApiControllerChannelInfoFilter> _logger;

    public WebApiControllerChannelInfoFilter(IDataExchangeLogger<WebApiControllerChannelInfoFilter> logger)
    {
        _logger = logger;
    }


    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var channelInfoForRequest = PopulateChannelInfoForRequest(context, true);
            await _logger.LogChannelInfo(channelInfoForRequest, context.HttpContext.RequestAborted);

            var actionExecutedContext = await next();

            var channelInfoForResponse = PopulateChannelInfoForResponse(actionExecutedContext, true);
            await _logger.LogChannelInfo(channelInfoForResponse, actionExecutedContext.HttpContext.RequestAborted);
        }
        catch (Exception ex)
        {
            await _logger.LogException(ex, context.HttpContext.RequestAborted, "WebApiControllerChannelInfoFilter");
            throw;
        }
    }

    private WebApiControllerChannelInfoForRequest PopulateChannelInfoForRequest(ActionExecutingContext context, bool includeRequestHeaders)
    {
        var httpContext = context.HttpContext;
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var requestIpResult = httpContext.GetRequestIp();
        var channelInfo = new WebApiControllerChannelInfoForRequest()
        {
            Headers = includeRequestHeaders ? ToDictionary(httpContext.Request.Headers) : null,
            ActionName = actionDescriptor != null ? actionDescriptor.ActionName : context.ActionDescriptor.DisplayName,
            ControllerName = actionDescriptor?.ControllerName,
            RequestUrl = httpContext.Request.GetDisplayUrl(),
            HttpMethod = httpContext.Request.Method,
            RequestIp = string.IsNullOrWhiteSpace(requestIpResult) ? null : requestIpResult,
            UserId = string.Empty
        };
        return channelInfo;
    }

    private static WebApiControllerChannelInfoForResponse PopulateChannelInfoForResponse(ActionExecutedContext context, bool includeHeaders)
    {
        if (context.HttpContext.Response != null && context.Result != null)
        {
            var statusCode = context.Result is ObjectResult objectResult && objectResult.StatusCode.HasValue ? objectResult.StatusCode.Value
                : context.Result is StatusCodeResult statusCodeResult ? statusCodeResult.StatusCode : context.HttpContext.Response.StatusCode;
            return new WebApiControllerChannelInfoForResponse()
            {
                StatusCode = statusCode,
                Status = GetStatusCodeString(statusCode),
                Headers = includeHeaders ? ToDictionary(context.HttpContext.Response.Headers) : null
            };
        }
        else
        {
            return new WebApiControllerChannelInfoForResponse()
            {
                Status = "Cannot get channel info for response.",
                StatusCode = 999
            };
        }
    }

    private static Dictionary<string, string>? ToDictionary(IEnumerable<KeyValuePair<string, StringValues>> col)
    {
        if (col == null)
        {
            return null;
        }
        var dict = new Dictionary<string, string>();
        foreach (var k in col)
        {
            dict.Add(k.Key, string.Join(", ", k.Value.ToArray()));
        }
        return dict;
    }

    private static string GetStatusCodeString(int statusCode)
    {
        var name = ((HttpStatusCode)statusCode).ToString();
        string[] words = Regex.Matches(name, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
            .OfType<Match>()
            .Select(m => m.Value)
            .ToArray();
        return words.Length == 0 ? name : string.Join(" ", words);
    }
}
