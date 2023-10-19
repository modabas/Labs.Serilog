using Serilog.WebApi.InterchangeContext.Core.Dto;

namespace Serilog.WebApi.InterchangeContext.Core.Services;

public interface IInterchangeContext
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string CurrentStep { get; set; }
    public DateTimeOffset CreatedAt { get; }
    Task SetProperty(ContextProperty property, CancellationToken cancellationToken);
    Task<IEnumerable<ContextProperty>> GetPropertiesForContentLog(CancellationToken cancellationToken);
    public IServiceProvider Services { get; }
}
