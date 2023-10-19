using Serilog.WebApi.DataExchangeLogger.Dto;

namespace Serilog.WebApi.DataExchangeLogger.Loggers;

public interface IDataExchangeLogger<TCaller>
{
    public Task LogContent<TLogContent>(TLogContent logContent, CancellationToken cancellationToken);
    public Task LogException(Exception? exception, CancellationToken cancellationToken, string? message, params object?[] args);
    public Task LogChannelInfo(ChannelInfo channelInfo, CancellationToken cancellationToken);
}
