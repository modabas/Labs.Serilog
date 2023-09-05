using MediatR;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.InterchangeContext.Mediatr;

public class InterchangeContextFactoryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IInterchangeContextAccessor _interchangeContextAccessor;
    private readonly IInterchangeContext _interchangeContext;

    public InterchangeContextFactoryBehavior(IInterchangeContextAccessor interchangeContextAccessor, IInterchangeContext interchangeContext)
    {
        _interchangeContextAccessor = interchangeContextAccessor;
        _interchangeContext = interchangeContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _interchangeContext.OpType = typeof(TRequest).FullName ?? string.Empty;
        _interchangeContextAccessor.InterchangeContext = _interchangeContext;
        return await next();
    }
}
