using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog.WebApi.InterchangeContext.Core.Services;
using Serilog.WebApi.ServiceStore;

namespace Serilog.WebApi.InterchangeContext.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterchangeContext(this IServiceCollection services)
    {
        services.TryAddScoped<IInterchangeContext, Services.InterchangeContext>();
        services.TryAddScoped<IInterchangeContextIdResolver, InterchangeContextIdResolver>();
        services.TryAddScoped<IServiceStore, ServiceStore.ServiceStore>();
        return services;
    }
}
