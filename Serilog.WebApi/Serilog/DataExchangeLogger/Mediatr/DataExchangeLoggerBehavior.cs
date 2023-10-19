using MediatR;
using Serilog.WebApi.Serilog.DataExchangeLogger.Loggers;

namespace Serilog.WebApi.Serilog.DataExchangeLogger.Mediatr;

//Marker class for IDataExchangeLogger caller
public class DataExchangeLoggerBehavior { }
public class DataExchangeLoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IDataExchangeLogger<DataExchangeLoggerBehavior> _logger;

    public DataExchangeLoggerBehavior(IDataExchangeLogger<DataExchangeLoggerBehavior> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await _logger.LogContent(request, cancellationToken);
            var response = await next();
            await _logger.LogContent(response, cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            await _logger.LogException(ex, cancellationToken, "DataExchangeLoggerBehavior");
            throw;
        }
    }
}