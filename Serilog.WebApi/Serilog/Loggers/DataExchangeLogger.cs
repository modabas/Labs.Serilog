using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.Serilog.Wrappers;

namespace Serilog.WebApi.Serilog.Loggers;

public class DataExchangeLogger<TCaller> : IDataExchangeLogger<TCaller>
{
    private readonly ILogger<TCaller> _logger;
    private readonly IInterchangeContextAccessor _interchangeContextAccessor;

    public DataExchangeLogger(ILogger<TCaller> logger, IInterchangeContextAccessor interchangeContextAccessor)
    {
        _logger = logger;
        _interchangeContextAccessor = interchangeContextAccessor;
    }

    public async Task LogInformation<TLogContent>(TLogContent logContent, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("{IsDataExchangeLog}", true))
        {
            var interchangeContext = _interchangeContextAccessor.InterchangeContext;
            Dictionary<string, object?>? interchangeContextData = null;
            if (interchangeContext is not null)
            {
                interchangeContextData = new Dictionary<string, object?>()
                {
                    { "OperationType", interchangeContext.OpType },
                    { "Id", interchangeContext.Id },
                    { "Properties", (await interchangeContext.GetPropertiesForContentLog(cancellationToken)).ToDictionary(p => p.Name, p => p.Value) }
                };
            }
            using (_logger.BeginScope(new Dictionary<string, object?>() { { "@InterchangeContext", interchangeContextData } }))
            {
                _logger.LogInformation("{@LogContent}", new DataExchangeLogWrapper<TLogContent>(logContent));
            }
        }
    }

    public Task LogError(Exception? exception, string? message, params object?[] args)
    {
        using (_logger.BeginScope("{IsDataExchangeLog}", true))
        {
            _logger.LogError(exception, message, args);
        }
        return Task.CompletedTask;
    }

}
