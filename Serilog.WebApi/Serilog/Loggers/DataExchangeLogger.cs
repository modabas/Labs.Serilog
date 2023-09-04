using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.Serilog.Wrappers;

namespace Serilog.WebApi.Serilog.Loggers;

public class DataExchangeLogger<TCaller> : IDataExchangeLogger<TCaller>
{
    private readonly ILogger<TCaller> _logger;
    private readonly IInterchangeContext _interchangeContext;

    public DataExchangeLogger(ILogger<TCaller> logger, IInterchangeContext interchangeContext)
    {
        _logger = logger;
        _interchangeContext = interchangeContext;
    }

    public async Task LogInformation<TLogContent>(TLogContent logContent, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("{IsDataExchangeLog}", true))
        {
            var properties = await _interchangeContext.GetPropertiesForContentLog(cancellationToken);
            var propertyBag = properties.ToDictionary(p => p.Name, p => p.Value);
            var interchangeContextData = new Dictionary<string, object?>() { { "OperationType", _interchangeContext.OpType }, { "Properties", propertyBag } };
            using (_logger.BeginScope(new Dictionary<string, object?>() { { "@InterchangeContext", interchangeContextData } }))
            {
                _logger.LogInformation("{@LogContent}", new DataExchangeLogWrapper<TLogContent>(logContent));
            }
        }
    }

    public async Task LogError(Exception? exception, string? message, params object?[] args)
    {
        using (_logger.BeginScope("{IsDataExchangeLog}", true))
        {
            _logger.LogError(exception, message, args);
        }
    }

}
