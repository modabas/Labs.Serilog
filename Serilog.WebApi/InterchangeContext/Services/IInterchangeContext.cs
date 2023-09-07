using Serilog.WebApi.InterchangeContext.Dto;
using Serilog.WebApi.ServiceStore;

namespace Serilog.WebApi.InterchangeContext.Services;

public interface IInterchangeContext
{
    public string Id { get; set; }
    public string OpType { get; set; }
    public string CurrentStep { get; set; }
    public DateTimeOffset CreatedAt { get; }
    Task SetProperty(ContextProperty property, CancellationToken cancellationToken);
    Task<IEnumerable<ContextProperty>> GetPropertiesForContentLog(CancellationToken cancellationToken);
    public IServiceProvider Services { get; }
}
