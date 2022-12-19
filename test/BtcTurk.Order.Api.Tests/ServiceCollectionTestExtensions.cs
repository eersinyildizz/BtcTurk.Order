using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace BtcTurk.Order.Api.Tests;

public static class ServiceCollectionTestExtensions
{
    public static bool IsRegistered<T>(this IServiceCollection services) {
        return services.Any(p=>p.ServiceType.Equals(typeof(T)));
    }

    public static bool IsImplementedClassRegistered<TImpl>(this IServiceCollection services)
    {
        return services.Any(p=>p.ImplementationType.Equals(typeof(TImpl)));
    }
}