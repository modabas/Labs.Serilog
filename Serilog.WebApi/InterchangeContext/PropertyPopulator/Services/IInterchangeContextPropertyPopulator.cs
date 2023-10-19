using Serilog.WebApi.InterchangeContext.Core.Dto;

namespace Serilog.WebApi.InterchangeContext.PropertyPopulator.Services;

public interface IInterchangeContextPropertyPopulator<T>
{
    Task<IEnumerable<ContextProperty>> GetProperties(T instance, CancellationToken cancellationToken);
    Task SetProperties(T instance, CancellationToken cancellationToken);
}
