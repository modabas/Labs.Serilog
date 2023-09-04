using Serilog.WrapAndTransform;

namespace Serilog.WebApi.Serilog.Wrappers;

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

