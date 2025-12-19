using Google.GenAI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WellInsightEngine.Core.Abstractions;
using WellInsightEngine.Core.Abstractions.Services.Ai;
using WellInsightEngine.Infrastructure.Services.Ai.Options;

namespace WellInsightEngine.Infrastructure.Services.Ai;

public static class DependencyInjection
{
    public static IServiceCollection AddAi(this IServiceCollection services)
    {
        services.ConfigureOptions<AiOptionsSetup>();
        services.AddSingleton<Client>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AiOptions>>().Value;
            return new Client(apiKey: options.ApiKey);
        });

        services.AddScoped<IGoogleAiService, GoogleAiService>();
        return services;
    }
}