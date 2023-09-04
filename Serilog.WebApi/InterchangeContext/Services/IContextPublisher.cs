namespace Serilog.WebApi.InterchangeContext.Services;

public interface IContextPublisher<T>
{
    Task<IEnumerable<ContextProperty>> GetPromotions(T instance, CancellationToken cancellationToken);
    Task CreatePromotions(T instance, CancellationToken cancellationToken);
}
