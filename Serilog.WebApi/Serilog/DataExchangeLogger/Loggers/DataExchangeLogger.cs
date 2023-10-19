using Microsoft.AspNetCore.Http.HttpResults;
using Serilog.Context;
using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.Serilog.DataExchangeLogger.Dto;
using Serilog.WebApi.Serilog.DataExchangeLogger.Wrappers;
using System.Threading;

namespace Serilog.WebApi.Serilog.DataExchangeLogger.Loggers;

public class DataExchangeLogger<TCaller> : IDataExchangeLogger<TCaller>
{
    private readonly ILogger<TCaller> _logger;
    private readonly IInterchangeContextAccessor _interchangeContextAccessor;

    public DataExchangeLogger(ILogger<TCaller> logger, IInterchangeContextAccessor interchangeContextAccessor)
    {
        _logger = logger;
        _interchangeContextAccessor = interchangeContextAccessor;
    }

    public async Task LogContent<TLogContent>(TLogContent logContent, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("{IsDataExchangeLog}, {LogType}", true, "Content"))
        {
            var interchangeContext = _interchangeContextAccessor.InterchangeContext;
            Dictionary<string, object?>? interchangeContextData = null;
            if (interchangeContext is not null)
            {
                interchangeContextData = new Dictionary<string, object?>()
                {
                    { "Name", interchangeContext.Name },
                    { "Id", interchangeContext.Id },
                    { "CurrentStep", interchangeContext.CurrentStep },
                    { "Properties", (await interchangeContext.GetPropertiesForContentLog(cancellationToken)).ToDictionary(p => p.Name, p => p.Value) }
                };
            }
            using (_logger.BeginScope(new Dictionary<string, object?>() { { "@InterchangeContext", interchangeContextData } }))
            {
                _logger.LogInformation("{@LogContent}", new DataExchangeLogWrapper<TLogContent>(logContent));
            }
        }
    }

    public Task LogException(Exception? exception, CancellationToken cancellationToken, string? message, params object?[] args)
    {
        using (_logger.BeginScope("{IsDataExchangeLog}, {LogType}", true, "Exception"))
        {
            var interchangeContext = _interchangeContextAccessor.InterchangeContext;
            Dictionary<string, object?>? interchangeContextData = null;
            if (interchangeContext is not null)
            {
                interchangeContextData = new Dictionary<string, object?>()
                {
                    { "Name", interchangeContext.Name },
                    { "Id", interchangeContext.Id },
                    { "CurrentStep", interchangeContext.CurrentStep },
                };
            }
            using (_logger.BeginScope(new Dictionary<string, object?>() { { "@InterchangeContext", interchangeContextData } }))
            {
                _logger.LogInformation(exception, message, args);
            }
        }
        return Task.CompletedTask;
    }

    public Task LogChannelInfo(ChannelInfo channelInfo, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("{IsDataExchangeLog}, {LogType}", true, "ChannelInfo"))
        {
            var interchangeContext = _interchangeContextAccessor.InterchangeContext;
            Dictionary<string, object?>? interchangeContextData = null;
            if (interchangeContext is not null)
            {
                interchangeContextData = new Dictionary<string, object?>()
                {
                    { "Name", interchangeContext.Name },
                    { "Id", interchangeContext.Id },
                };
            }
            using (_logger.BeginScope(new Dictionary<string, object?>() { { "@InterchangeContext", interchangeContextData } }))
            {
                _logger.LogInformation("{@ChannelInfo}", channelInfo);
            }
        }
        return Task.CompletedTask;
    }

}
