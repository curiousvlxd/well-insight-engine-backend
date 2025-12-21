using System.Text;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight.Payload;
using WellInsightEngine.Core.Extensions;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Services.WellInsightsAi;

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
            - highlights: 4–7 пунктів, тільки факт або спостереження.
              - НЕ додавай KPI-пункт, НЕ використовуй слово "KPI", НЕ виводь last/min/max/avg/mode/trend у вигляді KPI.
              - Не перераховуй метрики у форматі "min/max/avg=" або подібному.
              - У highlights пиши лише зрозумілі спостереження: події, режими роботи, стабільність сигналу,
                різкі зміни, плато, пропуски даних, аномалії, наявність/відсутність даних.
            - suspicions: 2–5 обережних гіпотез, без вигадування даних.
            - recommendedActions: 4–7 чітких і практичних кроків.

            - Якщо у часових рядах НЕМАЄ даних (усі points = 0):
              - summary має сказати, що телеметрія відсутня і аналіз неможливий.
              - highlights: 3–5 пунктів тільки про відсутність даних/покриття/вікно часу/події (без "зросло/знизилось").
              - suspicions: 1–2 гіпотези тільки про pipeline/сенсори/конфіг/часові фільтри.
              - recommendedActions: 4–7 кроків для відновлення збору даних (сенсор, інжест, мапінг, часові фільтри, доступи).
        """);

        sb.AppendLine();
        sb.AppendLine($"Ассет: {Safe(assetName) ?? "Н/Д"}");
        sb.AppendLine($"Свердловина: {wellName}");
        sb.AppendLine($"Період (UTC): {fromUtc:O} — {toUtc:O}");
        sb.AppendLine($"Інтервал агрегації: {interval.GetDescription()}");
        sb.AppendLine($"Кількість подій (well actions): {actions.Count}");
        sb.AppendLine();

        sb.AppendLine("Серії (time series):");
        foreach (var g in payload.Aggregations)
        {
            sb.AppendLine($"Група: тип={g.DataType.GetDescription()} | агрегація={g.Aggregation.GetDescription()} | параметрів={g.Parameters.Count}");

            foreach (var p in g.Parameters)
            {
                var pointsCount = p.DateTicks.Count;
                var (firstTs, firstVal) = First(p);
                var (lastTs, lastVal) = Last(p);

                sb.AppendLine(
                    $"- {p.ParameterName} | parameterId={p.ParameterId} | points={pointsCount} | " +
                    $"first=({firstTs},{firstVal}) | last=({lastTs},{lastVal})");
            }
        }

        if (payload.Kpis.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("KPI (обчислені):");
            foreach (var g in payload.Kpis)
            {
                sb.AppendLine($"- parameterId={g.ParameterId} | name={Safe(g.Name) ?? "n/a"} | agg={g.Aggregation}");
                foreach (var k in g.Items) 
                    sb.AppendLine($"  - kind={k.Kind.GetDescription()} | value={Safe(k.Value) ?? "n/a"}");
            }
        }

        sb.AppendLine();
        sb.AppendLine("Останні події:");
        foreach (var a in actions)
            sb.AppendLine($"- {a.Timestamp:O} | {Safe(a.Title) ?? "n/a"} | {Safe(a.Details) ?? string.Empty}");

        return sb.ToString();
    }

    private static (string Ts, string Val) First(WellInsightParameter p)
    {
        if (p.DateTicks.Count == 0)
            return ("n/a", "n/a");

        var t = p.DateTicks[0];
        return (t.Timestamp.ToString("O"), Safe(t.Value) ?? "n/a");
    }

    private static (string Ts, string Val) Last(WellInsightParameter p)
    {
        if (p.DateTicks.Count == 0)
            return ("n/a", "n/a");

        var t = p.DateTicks[^1];
        return (t.Timestamp.ToString("O"), Safe(t.Value) ?? "n/a");
    }

    private static string? Safe(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}