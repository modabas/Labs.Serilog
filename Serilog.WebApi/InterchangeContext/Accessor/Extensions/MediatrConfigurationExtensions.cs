using MediatR;
using Serilog.WebApi.InterchangeContext.Accessor.Mediatr;

namespace Serilog.WebApi.InterchangeContext.Accessor.Extensions;

public static class MediatrConfigurationExtensions
{
    public static MediatRServiceConfiguration AddInterchangeContextFactoryBehavior(this MediatRServiceConfiguration configuration)
    {
        configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InterchangeContextFactoryBehavior<,>));
        return configuration;
    }
}
