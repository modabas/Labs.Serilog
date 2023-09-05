using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.ServiceStore;

namespace Serilog.WebApi.InterchangeContext.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPropertyPopulator<TPopulator>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TPopulator : class
    {
        Type typeFromHandle = typeof(TPopulator);
        Type? type = typeFromHandle.GetInterfaces().FirstOrDefault((t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IInterchangeContextPropertyPopulator<>));
        if (type == null)
        {
            throw new AggregateException(typeFromHandle.Name + " does not implement IInterchangeContextPropertyPopulator<>.");
        }

        Type type2 = type!.GetGenericArguments().First();
        Type serviceType = typeof(IInterchangeContextPropertyPopulator<>).MakeGenericType(type2);
        services.Add(new ServiceDescriptor(serviceType, typeFromHandle, lifetime));
        return services;
    }

    public static IServiceCollection AddInterchangeContext(this IServiceCollection services)
    {
        services.AddSingleton<IInterchangeContextAccessor, InterchangeContextAccessor>();
        services.AddScoped<IInterchangeContext, Services.InterchangeContext>();
        services.AddScoped<IServiceStore, ServiceStore.ServiceStore>();
        return services;
    }
}
