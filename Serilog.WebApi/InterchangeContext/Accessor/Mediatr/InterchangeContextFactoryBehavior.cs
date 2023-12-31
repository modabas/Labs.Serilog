﻿using MediatR;
using Serilog.WebApi.InterchangeContext.Accessor.Services;
using Serilog.WebApi.InterchangeContext.Core.Services;

namespace Serilog.WebApi.InterchangeContext.Accessor.Mediatr;

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
        if (!_interchangeContextAccessor.IsInitialized)
        {
            _interchangeContext.Name = typeof(TRequest).FullName ?? string.Empty;
            _interchangeContextAccessor.InterchangeContext = _interchangeContext;
        }
        return await next();
    }
}
