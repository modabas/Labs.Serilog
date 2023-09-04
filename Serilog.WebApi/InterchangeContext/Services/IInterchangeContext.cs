namespace Serilog.WebApi.InterchangeContext.Services;

public interface IInterchangeContext
{
    public string Id { get; }
    public string OpType { get; }
    public DateTime CreatedAt { get; }
    public bool IsCreated { get; }
    Task Create(string id, string opType, CancellationToken cancellationToken);
    Task SetProperty(ContextProperty property, CancellationToken cancellationToken);
    Task<IEnumerable<ContextProperty>> GetPropertiesForContentLog(CancellationToken cancellationToken);
}
