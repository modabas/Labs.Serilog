using Serilog.WebApi.InterchangeContext.Dto;
using Serilog.WebApi.ServiceStore;
using System.Diagnostics;

namespace Serilog.WebApi.InterchangeContext.Services;

public class InterchangeContext : IInterchangeContext
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
    public string CurrentStep { get; set; } = string.Empty;

    public IServiceProvider Services => _serviceStore.ServiceProvider;

    public Dictionary<string, ContextProperty> PropertyBag = new();
    private readonly IServiceStore _serviceStore;

    public InterchangeContext(IServiceStore serviceStore, 
        IInterchangeContextIdResolver idResolver)
    {
        _serviceStore = serviceStore;
        Id = idResolver.ResolveId();
    }

    public Task SetProperty(ContextProperty property, CancellationToken cancellationToken)
    {
        PropertyBag[property.Name] = property;
        return Task.CompletedTask;
    }

    public Task<IEnumerable<ContextProperty>> GetPropertiesForContentLog(CancellationToken cancellationToken)
    {
        //Null checked in CheckMessageContextCreated
        var ret = PropertyBag.Where(p => p.Value.WriteToContentLog == true).Select(p => p.Value).ToList().AsReadOnly();
        return Task.FromResult<IEnumerable<ContextProperty>>(ret);
    }
}
