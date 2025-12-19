using Microsoft.Extensions.DependencyInjection;
using WellInsightEngine.Infrastructure.Persistence;
using WellInsightEngine.Infrastructure.Services.Ai;

namespace WellInsightEngine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistence();
        services.AddAi();
        return services;
    }
}