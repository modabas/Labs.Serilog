using Serilog.WrapAndTransform.Destructuring;

namespace Serilog.Console;

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

