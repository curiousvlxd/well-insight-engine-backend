using Microsoft.Extensions.DependencyInjection;
using WellInsightEngine.Infrastructure.Persistence;

namespace WellInsightEngine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistence();
        return services;
    }
}