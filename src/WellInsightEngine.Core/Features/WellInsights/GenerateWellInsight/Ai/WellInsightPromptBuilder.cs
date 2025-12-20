using System.Globalization;
using System.Text;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight.Payload;
using WellInsightEngine.Core.Extensions;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;

public static class WellInsightPromptBuilder
{
    public static string Build(string? assetName, string wellName, DateTimeOffset fromUtc, DateTimeOffset toUtc, GroupingInterval interval, WellInsightPayload payload, IReadOnlyList<WellAction> actions)
    {
        var sb = new StringBuilder();

        sb.AppendLine("""
            Ти — аналітик промислового моніторингу свердловин.
            Поверни ВИКЛЮЧНО валідний JSON.
            НЕ використовуй ```json, ``` або будь-які інші маркдауни.
            НЕ додавай жодного тексту поза JSON.
            Відповідь повинна починатися з { і закінчуватися }.

            Схема відповіді:
            {
              "title": "string",
              "summary": "string",
              "highlights": ["string", "..."],
              "suspicions": ["string", "..."],
              "recommendedActions": ["string", "..."]
            }

            Правила:
            - Пиши українською.
            - Заголовок має містити назву свердловини і (за наявності) Ассет.
            - Summary: 2–4 речення.
            - highlights: 4–7 пунктів, тільки факт/спостереження.
            - suspicions: 2–5 обережних гіпотез, без вигадування даних.
            - recommendedActions: 4–7 чітких кроків.
        """);

        sb.AppendLine($"Ассет: {Safe(assetName) ?? "Н/Д"}");
        sb.AppendLine($"Свердловина: {wellName}");
        sb.AppendLine($"Період (UTC): {fromUtc:O} — {toUtc:O}");
        sb.AppendLine($"Інтервал агрегації: {interval.GetDescription()}");
        sb.AppendLine($"Кількість подій (well actions): {actions.Count}");
        sb.AppendLine();
        sb.AppendLine("Серії (time series):");
        foreach (var s in payload.Parameters)
        {
            var pointsCount = s.Metrics.Count;
            var (firstTs, firstVal) = FirstParameter(s);
            var (lastTs, lastVal) = LastParameter(s);

            sb.AppendLine(
                $"- {s.Name} | тип={s.DataType} | агрегація={s.Aggregation} | points={pointsCount} | " +
                $"first=({firstTs},{firstVal}) | last=({lastTs},{lastVal})");
        }

        sb.AppendLine();
        sb.AppendLine("Останні події:");
        foreach (var a in actions.Take(30))
            sb.AppendLine($"- {a.Timestamp:O} | {a.Title} | {a.Details}");

        return sb.ToString();
    }

    private static (string Ts, string Val) FirstParameter(WellInsightParameter s)
    {
        if (s.Metrics.Count == 0) return ("n/a", "n/a");
        var p = s.Metrics[0];
        return (p.Timestamp.ToString("O", CultureInfo.InvariantCulture), Safe(p.Value) ?? "n/a");
    }

    private static (string Ts, string Val) LastParameter(WellInsightParameter s)
    {
        if (s.Metrics.Count == 0) return ("n/a", "n/a");
        var p = s.Metrics[^1];
        return (p.Timestamp.ToString("O", CultureInfo.InvariantCulture), Safe(p.Value) ?? "n/a");
    }

    private static string? Safe(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
