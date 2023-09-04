using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.InterchangeContext;

public static class ContextExtensions
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
}
