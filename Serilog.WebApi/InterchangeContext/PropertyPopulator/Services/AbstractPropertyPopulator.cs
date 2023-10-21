using Serilog.WebApi.InterchangeContext.Core.Dto;

namespace Serilog.WebApi.InterchangeContext.PropertyPopulator.Services;

public abstract class AbstractPropertyPopulator<T> : IInterchangeContextPropertyPopulator<T>
{
    private readonly Dictionary<string, ContextProperty> _properties = new();

    public abstract Task SetProperties(T instance, CancellationToken cancellationToken);

    public Task<IEnumerable<ContextProperty>> GetProperties(CancellationToken cancellationToken)
    {
        return Task.FromResult(_properties.Values.AsEnumerable());
    }

    protected ContextProperty SetProperty(string name, object? value, bool writeToContentLog = true)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        var property = new ContextProperty() { Name = name, Value = value, WriteToContentLog = writeToContentLog };
        _properties[property.Name] = property;
        return property;
    }
}
