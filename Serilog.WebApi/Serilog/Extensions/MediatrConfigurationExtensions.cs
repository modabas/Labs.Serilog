using MediatR;
using Serilog.WebApi.Serilog.Mediatr;

namespace Serilog.WebApi.Serilog.Extensions;

public static class MediatrConfigurationExtensions
{
    public static MediatRServiceConfiguration AddDataExchangeLoggerBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(DataExchangeLoggerBehavior<,>));
        return configuration;
    }
}
