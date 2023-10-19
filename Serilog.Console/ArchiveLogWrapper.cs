using Serilog.WrapAndTransform.Destructuring;

namespace Serilog.Console;

internal class ArchiveLogWrapper<T> : ILogWrapper<T>
{
    public T? Log { get; set; }

    public ArchiveLogWrapper()
    {
    }

    public ArchiveLogWrapper(T log)
    {
        Log = log;
    }
}

