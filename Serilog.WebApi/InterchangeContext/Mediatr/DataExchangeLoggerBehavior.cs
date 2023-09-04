using MediatR;
using Serilog.WebApi.Serilog.Loggers;

namespace Serilog.WebApi.InterchangeContext.Mediatr;

public class DataExchangeLoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IDataExchangeLogger<TRequest> _logger;

    public DataExchangeLoggerBehavior(IDataExchangeLogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await _logger.LogInformation(request, cancellationToken);
            var response = await next();
            await _logger.LogInformation(response, cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            await _logger.LogError(ex, "DataExchangeLoggerBehavior");
            throw;
        }
    }
}