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
              - РІВНО 1 пункт може бути про KPI.
              - KPI-пункт ОБОВʼЯЗКОВО в одному рядку.
              - KPI має бути ЛЮДИНОЧИТАЄМИМ: без технічних слів last/avg/min/max/trend.

              - ЄДИНИЙ формат KPI-пункту (без винятків):
                "KPI: <прикметник агрегації> <метрика> <значення> (<відхилення%>); <прикметник агрегації> <метрика> <значення> (<відхилення%>); ..."
            
              - Назва KPI ЗАВЖДИ у формі:
                "<прикметник агрегації> <метрика>"
                де <прикметник агрегації> — прикметник, а НЕ іменник.
              - Прикметник агрегації ОБОВʼЯЗКОВО граматично узгоджується з родом метрики.
              - Рід визначається за головним іменником метрики.
              - Будь-яке порушення узгодження роду прикметника з метрикою ЗАБОРОНЕНО.

              - Дозволені прикметники агрегації (перед метрикою):
                - середня / середній / середнє
                - мінімальна / мінімальний / мінімальне
                - максимальна / максимальний / максимальне

              - ЗАБОРОНЕНО:
                - "<тип агрегації> <метрика>"
                - метрика з великої літери в KPI (пиши з малої)

              - Тип агрегації у дужках ОБОВʼЯЗКОВИЙ для КОЖНОГО KPI,
                навіть якщо агрегація вже присутня у прикметнику.

              - Відхилення (%):
                - зі знаком + або -
                - без слова "trend"

              - Якщо відхилення неможливо обчислити —
                пиши "(<тип агрегації>, n/a)".

              - Якщо KPI взагалі немає або немає даних —
                KPI-пункт: "KPI: n/a".

            - Інші highlights НЕ повинні бути про KPI.
              Використовуй: події, режими роботи, стабільність сигналу,
              різкі зміни, плато, пропуски даних, аномалії,
              наявність або відсутність даних.

            - suspicions: 2–5 обережних гіпотез, без вигадування даних.

            - recommendedActions: 4–7 чітких і практичних кроків.

            - Якщо у часових рядах НЕМАЄ даних (усі points = 0):
              - summary має прямо сказати, що телеметрія відсутня
                і аналіз трендів або аномалій неможливий.
              - highlights: 3–5 пунктів ТІЛЬКИ про факт відсутності даних,
                покриття, вікно часу або події
                (без формулювань "зросло" або "знизилось").
              - KPI-пункт:
                - або ВЗАГАЛІ не додавай KPI-пункт,
                - або (якщо він є) кожен KPI має бути ТІЛЬКИ "n/a"
                  без відсотків або трендів.
              - suspicions: максимум 1–2 короткі гіпотези,
                тільки про pipeline, сенсори, конфігурацію
                або часові фільтри,
                без тверджень про роботу свердловини.
              - recommendedActions: 4–7 кроків,
                сфокусовані на перевірці збору даних
                (сенсор, інжест, мапінг параметрів,
                 часові фільтри, доступи).
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
            sb.AppendLine($"Група: тип={g.DataType} | агрегація={g.Aggregation} | параметрів={g.Parameters.Count}");

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