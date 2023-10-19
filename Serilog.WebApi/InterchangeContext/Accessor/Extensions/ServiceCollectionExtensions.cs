using Serilog.WebApi.InterchangeContext.Accessor.Services;
using Serilog.WebApi.InterchangeContext.Core.Extensions;

namespace Serilog.WebApi.InterchangeContext.Accessor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterchangeContextAccessor(this IServiceCollection services)
    {
        services.AddSingleton<IInterchangeContextAccessor, InterchangeContextAccessor>();
        services.AddInterchangeContext();
        return services;
    }
}
