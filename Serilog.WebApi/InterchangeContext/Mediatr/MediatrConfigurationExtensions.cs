using MediatR;

namespace Serilog.WebApi.InterchangeContext.Mediatr;

public static class MediatrConfigurationExtensions
{
    public static MediatRServiceConfiguration AddInterchangeContextBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InterchangeContextBehavior<,>));
        return configuration;
    }

    public static MediatRServiceConfiguration AddDataExchangeLoggerBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(DataExchangeLoggerBehavior<,>));
        return configuration;
    }
}
