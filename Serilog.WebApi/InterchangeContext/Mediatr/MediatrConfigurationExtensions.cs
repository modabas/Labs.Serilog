using MediatR;

namespace Serilog.WebApi.InterchangeContext.Mediatr;

public static class MediatrConfigurationExtensions
{
    public static MediatRServiceConfiguration AddInterchangeContextFactoryBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InterchangeContextFactoryBehavior<,>));
        return configuration;
    }

    public static MediatRServiceConfiguration AddInterchangeContextPopulateRequestPropertiesBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InterchangeContextPopulateRequestPropertiesBehavior<,>));
        return configuration;
    }

    public static MediatRServiceConfiguration AddInterchangeContextPopulateResponsePropertiesBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InterchangeContextPopulateResponsePropertiesBehavior<,>));
        return configuration;
    }

    public static MediatRServiceConfiguration AddDataExchangeLoggerBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(DataExchangeLoggerBehavior<,>));
        return configuration;
    }
}
