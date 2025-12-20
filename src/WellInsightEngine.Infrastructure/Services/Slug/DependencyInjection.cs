using Microsoft.Extensions.DependencyInjection;
using WellInsightEngine.Core.Abstractions.Services.Slug;

namespace WellInsightEngine.Infrastructure.Services.Slug;

public static class AuthenticationDependencyInjection
{
    public static IServiceCollection AddSlug(this IServiceCollection services)
    {
        services.AddSingleton<ISlugService, SlugService>();
        return services;
    }
}