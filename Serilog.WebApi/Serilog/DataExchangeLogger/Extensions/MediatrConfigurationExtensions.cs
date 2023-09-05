using MediatR;
using Serilog.WebApi.Serilog.DataExchangeLogger.Mediatr;

namespace Serilog.WebApi.Serilog.DataExchangeLogger.Extensions;

public static class MediatrConfigurationExtensions
{
    public static MediatRServiceConfiguration AddDataExchangeLoggerBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(DataExchangeLoggerBehavior<,>));
        return configuration;
    }
}
