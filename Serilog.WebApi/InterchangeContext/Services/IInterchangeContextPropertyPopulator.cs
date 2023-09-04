namespace Serilog.WebApi.InterchangeContext.Services;

public interface IInterchangeContextPropertyPopulator<T>
{
    Task<IEnumerable<ContextProperty>> GetProperties(T instance, CancellationToken cancellationToken);
    Task AddProperties(T instance, CancellationToken cancellationToken);
}
