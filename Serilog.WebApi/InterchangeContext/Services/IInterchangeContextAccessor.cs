namespace Serilog.WebApi.InterchangeContext.Services;

public interface IInterchangeContextAccessor
{
    IInterchangeContext? InterchangeContext { get; set; }
}
