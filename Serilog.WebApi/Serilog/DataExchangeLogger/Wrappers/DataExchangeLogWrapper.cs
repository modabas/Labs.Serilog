using Serilog.WrapAndTransform;

namespace Serilog.WebApi.Serilog.DataExchangeLogger.Wrappers;

internal class DataExchangeLogWrapper<T> : ILogWrapper<T>
{
    public T? Log { get; set; }

    public DataExchangeLogWrapper()
    {
    }

    public DataExchangeLogWrapper(T log)
    {
        Log = log;
    }
}

