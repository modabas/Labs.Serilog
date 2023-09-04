using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.InterchangeContext;

public static class ContextExtensions
{
    public static IServiceCollection AddPublisher<TPublisher>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TPublisher : class
    {
        Type typeFromHandle = typeof(TPublisher);
        Type? type = typeFromHandle.GetInterfaces().FirstOrDefault((t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IContextPublisher<>));
        if (type == null)
        {
            throw new AggregateException(typeFromHandle.Name + " does not implement IContextPublisher<>.");
        }

        Type type2 = type!.GetGenericArguments().First();
        Type serviceType = typeof(IContextPublisher<>).MakeGenericType(type2);
        services.Add(new ServiceDescriptor(serviceType, typeFromHandle, lifetime));
        return services;
    }
}
