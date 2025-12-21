using WellInsightEngine.Core.Abstractions.Services.WellInsightsAi.Contracts;
using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Features.WellMetrics;
using WellInsightEngine.Core.Services.WellInsightsAi;

namespace WellInsightEngine.Core.Abstractions.Services.WellInsightsAi;

public interface IWellInsightsAiService
{
    Task<GenerateWellInsightAiResponse> GenerateAsync(GenerateWellInsightAiRequest request, CancellationToken cancellation = default);
    WellMetricAggregationPlan ResolvePlan(DateTimeOffset fromUtc, DateTimeOffset toUtc);
}