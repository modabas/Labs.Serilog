using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.Serilog.Wrappers;

namespace Serilog.WebApi.Serilog.Loggers;

public class AuditLogger<TCaller> : IAuditLogger<TCaller>
{
    private readonly ILogger<TCaller> _logger;
    private readonly IInterchangeContext _interchangeContext;

    public AuditLogger(ILogger<TCaller> logger, IInterchangeContext interchangeContext)
    {
        _logger = logger;
        _interchangeContext = interchangeContext;
    }

    public async Task LogInformation<TLogContent>(TLogContent logContent, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("{IsAuditLog}", true))
        {
            var properties = await _interchangeContext.GetPropertiesForContentLog(cancellationToken);
            var propertyBag = new Dictionary<string, object?>();
            foreach (var property in properties)
            {
                propertyBag[$"{property.Name}"] = property.Value;
            }
            using (_logger.BeginScope(new Dictionary<string, object?>() { { "@InterchangeContext", propertyBag } }))
            {
                _logger.LogInformation("{@LogContent}", new AuditLogWrapper<TLogContent>(logContent));
            }
        }
    }

    public async Task LogError(Exception? exception, string? message, params object?[] args)
    {
        using (_logger.BeginScope("{IsAuditLog}", true))
        {
            _logger.LogError(exception, message, args);
        }
    }

}
