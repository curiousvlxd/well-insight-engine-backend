using Microsoft.Extensions.Options;
using WellInsightEngine.Core.Abstractions.Services.Ai;
using WellInsightEngine.Core.Abstractions.Services.WellInsightsAi;
using WellInsightEngine.Core.Abstractions.Services.WellInsightsAi.Contracts;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight.Payload;
using WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;
using WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai.Options;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Services.WellInsightsAi;

public sealed class WellInsightsAiService(IGoogleAiService ai, IOptions<WellInsightsAiOptions> options) : IWellInsightsAiService
{
    private readonly WellInsightsAiOptions _options = options.Value;

    public async Task<GenerateWellInsightAiResponse> GenerateAsync(GenerateWellInsightAiRequest request, CancellationToken cancellation = default)
    {
        var (payload, actions, fromUtc, interval, toUtc, well) = request;

        payload = WellInsightPayload.Create(TrimAggregations(payload.Aggregations, _options));
        actions = TrimActions(actions, _options).ToList();
        var prompt = WellInsightPromptBuilder.Build(well.Asset?.Name, well.Name, fromUtc, toUtc, interval, payload, actions);
        
        if (prompt.Length > _options.PromptMaxChars)
            prompt = prompt[.._options.PromptMaxChars];

        var aiResponse = await ai.GenerateAsync(prompt, cancellation);
        var envelope = GenerateWellInsightMapper.ToEnvelope(aiResponse);
        envelope.Normalize(well, interval);
        return GenerateWellInsightAiResponse.Create(envelope, payload, actions);
    }

    public WellMetricAggregationPlan ResolvePlan(DateTimeOffset fromUtc, DateTimeOffset toUtc)
    {
        var interval = WellInsightRules.ChooseInterval(fromUtc, toUtc, _options.MaxPointsPerSeries);
        var plan = WellInsightKnowledge.ResolvePlan(interval, _options.AggregationTypes);
        return plan;
    }

    private static IEnumerable<WellInsightAggregation> TrimAggregations(
        IReadOnlyList<WellInsightAggregation> aggregations, WellInsightsAiOptions o)
        => aggregations
            .Select(g => WellInsightAggregation.Create(
                g.DataType,
                g.Aggregation,
                g.Parameters
                    .Select(p => WellInsightParameter.Create(p.WellId, p.ParameterId, p.ParameterName, TrimTicks(p.DateTicks, o.MaxPointsPerSeries).ToList()))
                    .OrderByDescending(p => p.DateTicks.Count)
                    .Take(o.TopKSeriesForPrompt)
                    .ToList()));

    private static IEnumerable<WellAction> TrimActions(List<WellAction> actions, WellInsightsAiOptions o)
        => actions.Count <= o.MaxActionsForPrompt
            ? actions
            : actions
                .OrderByDescending(x => x.Timestamp)
                .Take(o.MaxActionsForPrompt)
                .OrderBy(x => x.Timestamp);

    private static IEnumerable<WellInsightMetric> TrimTicks(IReadOnlyList<WellInsightMetric> ticks, int maxPoints)
        => ticks.Count <= maxPoints
            ? ticks
            : Downsample(ticks, maxPoints);

    private static IEnumerable<WellInsightMetric> Downsample(IReadOnlyList<WellInsightMetric> ticks, int maxPoints)
    {
        if (maxPoints <= 0 || ticks.Count == 0)
            return [];

        if (ticks.Count <= maxPoints)
            return ticks;

        if (maxPoints == 1)
            return [ticks[^1]];

        var step = (double)(ticks.Count - 1) / (maxPoints - 1);
        var result = new List<WellInsightMetric>(maxPoints);

        for (var i = 0; i < maxPoints; i++)
        {
            var idx = (int)Math.Round(i * step);
            if (idx < 0) idx = 0;
            if (idx >= ticks.Count) idx = ticks.Count - 1;
            result.Add(ticks[idx]);
        }

        return result;
    }
}
