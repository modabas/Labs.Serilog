using Serilog.Context;
using Serilog.WrapAndTransform;

namespace Serilog.Console;
internal class AuditLogger
{
    private readonly object _propertyBag;
    public AuditLogger(object propertyBag)
    {
        _propertyBag = propertyBag;
    }
    public void LogInformation<TLogContent>(TLogContent logContent)
    {
        using (LogContext.PushProperty("IsAuditLog", true))
        {
            using (LogContext.PushProperty("Namespace", typeof(LogHelper).FullName))
            {
                using (LogContext.PushProperty("PropertyBag", _propertyBag, true))
                {
                    Log.Information("{@LogContent}", new AuditLogWrapper<TLogContent>(logContent));
                }
            }
        }
    }
}
