using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WellInsightEngine.Core.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly
            .DefinedTypes
            .Where(t => t is { IsAbstract: false, IsInterface: false } &&
                        t.Name.EndsWith("Feature", StringComparison.Ordinal));

        foreach (var t in types)
            services.TryAddScoped(t.AsType());

        return services;
    }
}