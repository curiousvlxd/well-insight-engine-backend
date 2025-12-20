using System.Text;
using WellInsightEngine.Core.Entities.Insight.Payload;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;


public static class WellInsightPromptBuilder
{
    public static string Build(
        Guid wellId,
        DateTimeOffset fromUtc,
        DateTimeOffset toUtc,
        GroupingInterval interval,
        InsightPayload payload)
    {
        var sb = new StringBuilder();

        sb.AppendLine("""
                      You are an industrial well monitoring analyst.
                      Return ONLY valid JSON with fields:
                      title, summary, highlights, suspicions, recommendedActions.
                      Use clear technical language.
                      """);

        sb.AppendLine($"WellId: {wellId}");
        sb.AppendLine($"Period: {fromUtc:O} — {toUtc:O}");
        sb.AppendLine($"Aggregation interval: {interval}");
        sb.AppendLine($"Events count: {payload.Events.Count}");

        sb.AppendLine("Time series:");

        foreach (var s in payload.Series)
        {
            var first = s.Points.FirstOrDefault()?.Value ?? "n/a";
            var last = s.Points.LastOrDefault()?.Value ?? "n/a";

            sb.AppendLine(
                $"- {s.Name}: {WellInsightKnowledge.HumanDataType(s.DataType)}, " +
                $"{WellInsightKnowledge.HumanAggregation(s.Aggregation)}, " +
                $"points={s.Points.Count}, first={first}, last={last}");
        }

        sb.AppendLine("Recent events:");

        foreach (var e in payload.Events.Take(25))
            sb.AppendLine($"- {e.Timestamp:O}: {e.Title}. {e.Details}");

        return sb.ToString();
    }
}