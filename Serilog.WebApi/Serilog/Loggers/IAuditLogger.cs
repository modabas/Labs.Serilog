namespace Serilog.WebApi.Serilog.Loggers;

public interface IAuditLogger<TCaller>
{
    public Task LogInformation<TLogContent>(TLogContent logContent, CancellationToken cancellationToken);
    public Task LogError(Exception? exception, string? message, params object?[] args);
}
