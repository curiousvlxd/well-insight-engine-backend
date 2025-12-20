using Microsoft.Extensions.DependencyInjection;
using WellInsightEngine.Infrastructure.Persistence;
using WellInsightEngine.Infrastructure.Services.Ai;
using WellInsightEngine.Infrastructure.Services.Auth;
using WellInsightEngine.Infrastructure.Services.Slug;

namespace WellInsightEngine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistence();
        services.AddAi();
        services.AddKlerk();
        services.AddSlug();
        return services;
    }
}