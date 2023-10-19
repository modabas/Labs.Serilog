using MediatR;
using Serilog.WebApi.InterchangeContext.PropertyPopulator.Mediatr;

namespace Serilog.WebApi.InterchangeContext.PropertyPopulator.Extensions;

public static class MediatrConfigurationExtensions
{
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
}
