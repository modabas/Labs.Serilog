using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;

namespace Serilog.WrapAndTransform;
public class LogWrapperDestructuringPolicy<TLogWrapper, TLogContent> : IDestructuringPolicy where TLogWrapper : ILogWrapper<TLogContent>
{
    private readonly Func<TLogContent, object?> _transformation;
    private readonly Type _targetType;

    public LogWrapperDestructuringPolicy(Func<TLogContent, object?> transformation)
    {
        _targetType = typeof(TLogWrapper);
        _transformation = transformation;
    }

    public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result)
    {
        if (value == null || value.GetType() != _targetType)
        {
            result = null;
            return false;
        }

        var logWrapper = (TLogWrapper)value;
        if (logWrapper.Log is null)
        {
            result = null;
            return false;
        }

        var projected = _transformation(logWrapper.Log);
        result = propertyValueFactory.CreatePropertyValue(projected, destructureObjects: true);
        return true;
    }
}
