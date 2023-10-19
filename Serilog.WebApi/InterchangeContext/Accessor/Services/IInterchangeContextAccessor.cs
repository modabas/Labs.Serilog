using Serilog.WebApi.InterchangeContext.Core.Services;

namespace Serilog.WebApi.InterchangeContext.Accessor.Services;

public interface IInterchangeContextAccessor
{
    IInterchangeContext? InterchangeContext { get; set; }
    bool IsInitialized { get; }
}
