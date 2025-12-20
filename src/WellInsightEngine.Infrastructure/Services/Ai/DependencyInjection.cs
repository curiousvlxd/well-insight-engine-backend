using Google.Apis.Auth.OAuth2;
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
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<AiOptions>>().Value;

        if (options.Disabled)
        {   
            services.AddScoped<IGoogleAiService, DisabledGoogleAiService>();
            return services;
        }
        
        services.AddSingleton<Client>(_ => new Client(vertexAI: true, project: options.ProjectId, location: options.Location, credential: GoogleCredential.GetApplicationDefault()));
        services.AddScoped<IGoogleAiService, GoogleAiService>();
        return services;
    }
}