using Microsoft.Extensions.DependencyInjection;
using WellInsightEngine.Core.Features;
using WellInsightEngine.Core.Services.WellInsightsAi;

namespace WellInsightEngine.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddFeatures(AssemblyReference.Assembly);
        services.AddWellInsightsAi();
        return services;
    }
}