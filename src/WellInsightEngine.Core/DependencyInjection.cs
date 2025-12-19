using Microsoft.Extensions.DependencyInjection;
using WellInsightEngine.Core.Features;

namespace WellInsightEngine.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddFeatures(AssemblyReference.Assembly);
        return services;
    }
}