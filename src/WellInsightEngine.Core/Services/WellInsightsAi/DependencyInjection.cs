using Microsoft.Extensions.DependencyInjection;
using WellInsightEngine.Core.Abstractions.Services.WellInsightsAi;
using WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai.Options;

namespace WellInsightEngine.Core.Services.WellInsightsAi;

public static class DependencyInjection
{
    public static IServiceCollection AddWellInsightsAi(this IServiceCollection services)
    {
        services.ConfigureOptions<WellInsightsAiOptionsSetup>();
        services.AddScoped<IWellInsightsAiService, WellInsightsAiService>();
        return services;
    }
}