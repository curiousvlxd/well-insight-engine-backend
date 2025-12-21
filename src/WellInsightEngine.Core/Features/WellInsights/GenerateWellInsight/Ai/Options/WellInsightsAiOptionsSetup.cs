using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai.Options;

public sealed class WellInsightsAiOptionsSetup(IConfiguration configuration) : IConfigureOptions<WellInsightsAiOptions>
{
    public void Configure(WellInsightsAiOptions options)
    {
        configuration.GetSection(" WellInsightsAi").Bind(options);
    }
}