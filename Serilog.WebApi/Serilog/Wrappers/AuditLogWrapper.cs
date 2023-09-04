using Serilog.WrapAndTransform;

namespace Serilog.WebApi.Serilog.Wrappers;

internal class AuditLogWrapper<T> : ILogWrapper<T>
{
    public T? Log { get; set; }

    public AuditLogWrapper()
    {
    }

    public AuditLogWrapper(T log)
    {
        Log = log;
    }
}

