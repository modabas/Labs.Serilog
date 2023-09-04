using Serilog.WebApi.InterchangeContext.Dto;

namespace Serilog.WebApi.InterchangeContext.Services;

public interface IInterchangeContextPropertyPopulator<T>
{
    Task<IEnumerable<ContextProperty>> GetProperties(T instance, CancellationToken cancellationToken);
    Task SetProperties(T instance, CancellationToken cancellationToken);
}
