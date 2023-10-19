using MediatR;
using Serilog.WebApi.DataExchangeLogger.Mediatr;

namespace Serilog.WebApi.DataExchangeLogger.Extensions;

public static class MediatrConfigurationExtensions
{
    public static MediatRServiceConfiguration AddDataExchangeLoggerBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(DataExchangeLoggerBehavior<,>));
        return configuration;
    }
}
