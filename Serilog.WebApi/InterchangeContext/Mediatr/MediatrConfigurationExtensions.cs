using MediatR;

namespace Serilog.WebApi.InterchangeContext.Mediatr;

public static class MediatrConfigurationExtensions
{
    public static MediatRServiceConfiguration AddInterchangeContextBehaviorForRequest(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InterchangeContextBehaviorForRequest<,>));
        return configuration;
    }

    public static MediatRServiceConfiguration AddInterchangeContextBehaviorForResponse(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InterchangeContextBehaviorForResponse<,>));
        return configuration;
    }

    public static MediatRServiceConfiguration AddDataExchangeLoggerBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(DataExchangeLoggerBehavior<,>));
        return configuration;
    }
}
