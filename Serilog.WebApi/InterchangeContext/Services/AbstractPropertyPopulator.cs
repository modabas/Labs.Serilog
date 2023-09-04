namespace Serilog.WebApi.InterchangeContext.Services;

public abstract class AbstractPropertyPopulator<T> : IInterchangeContextPropertyPopulator<T>
{
    private readonly List<ContextProperty> _properties = new List<ContextProperty>();

    public abstract Task AddProperties(T instance, CancellationToken cancellationToken);

    public Task<IEnumerable<ContextProperty>> GetProperties(T instance, CancellationToken cancellationToken)
    {
        return Task.FromResult(_properties.AsEnumerable());
    }

    public ContextProperty AddProperty(string name, object? value, bool writeToContentLog = true)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        var property = new ContextProperty() { Name = name, Value = value, WriteToContentLog = writeToContentLog };
        _properties.Add(property);
        return property;
    }
}
