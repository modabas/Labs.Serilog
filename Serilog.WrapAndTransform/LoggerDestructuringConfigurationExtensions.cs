using Serilog;
using Serilog.Configuration;

namespace Serilog.WrapAndTransform;
public static class LoggerDestructuringConfigurationExtensions
{
    public static LoggerConfiguration ByTransformingWrapped<TLogWrapper, TLogContent>(this LoggerDestructuringConfiguration ldc, Func<TLogContent, object?> transformation) where TLogWrapper : ILogWrapper<TLogContent>
    {
        var policy = new LogWrapperDestructuringPolicy<TLogWrapper, TLogContent>(transformation);
        return ldc.With(policy);
    }
}
