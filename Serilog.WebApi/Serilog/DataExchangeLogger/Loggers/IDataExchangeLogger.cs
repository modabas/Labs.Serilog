namespace Serilog.WebApi.Serilog.DataExchangeLogger.Loggers;

public interface IDataExchangeLogger<TCaller>
{
    public Task LogInformation<TLogContent>(TLogContent logContent, CancellationToken cancellationToken);
    public Task LogError(Exception? exception, string? message, params object?[] args);
}
