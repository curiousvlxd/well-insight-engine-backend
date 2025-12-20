using System.Globalization;
using System.Text.Json;
using Dapper;
using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Abstractions.Services.Ai;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.Insight;
using WellInsightEngine.Core.Entities.Insight.Payload;
using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;

public sealed class GenerateWellInsightFeature(
    ISqlConnectionFactory sqlFactory,
    IApplicationDbContext context,
    IGoogleAiService ai)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<GenerateWellInsightResponse> Handle(GenerateWellInsightRequest request, CancellationToken ct)
    {
        var fromUtc = request.From.ToUniversalTime();
        var toUtc = request.To.ToUniversalTime();

        var parameters = await LoadParametersAsync(request.ParameterIds, ct);
        var actions = await LoadActionsAsync(request.WellId, fromUtc, toUtc, ct);

        var interval = WellInsightRules.ChooseInterval(fromUtc, toUtc, request.MaxPointsPerSeries);
        if (!WellMetricsAggregations.Map.TryGetValue(interval, out var view))
            throw new InvalidOperationException($"Unsupported interval: {interval}");

        var series = await FetchSeriesAsync(view, request.WellId, request.ParameterIds, fromUtc, toUtc, parameters, ct);

        var payload = new InsightPayload
        {
            Series = series,
            Events = actions.Select(ToEvent).ToList(),
            Kpis = BuildKpis(series)
        };

        var aiResponse = await GenerateAiAsync(request.WellId, fromUtc, toUtc, interval, payload, ct);

        var insight = new Insight
        {
            Id = Guid.NewGuid(),
            WellId = request.WellId,
            CreatedAt = DateTimeOffset.UtcNow,
            From = fromUtc,
            To = toUtc,
            Title = aiResponse.Title,
            Summary = aiResponse.Summary,
            Payload = JsonSerializer.Serialize(payload, JsonOptions)
        };

        context.Add(insight);

        foreach (var a in actions)
        {
            context.Add(new InsightAction
            {
                InsightId = insight.Id,
                WellActionId = a.Id
            });
        }

        await context.SaveChangesAsync(ct);

        return new GenerateWellInsightResponse
        {
            InsightId = insight.Id,
            WellId = insight.WellId,
            CreatedAt = insight.CreatedAt,
            From = insight.From,
            To = insight.To,
            Title = insight.Title,
            Summary = insight.Summary,
            Highlights = aiResponse.Highlights,
            Suspicions = aiResponse.Suspicions,
            RecommendedActions = aiResponse.RecommendedActions,
            Payload = payload
        };
    }

    private async Task<IReadOnlyDictionary<Guid, ParameterMeta>> LoadParametersAsync(Guid[] parameterIds, CancellationToken ct)
    {
        var ids = parameterIds.AsEnumerable();

        var list = await context.Parameters
            .Where(p => ids.Contains(p.Id))
            .Select(p => new ParameterMeta
            {
                Id = p.Id,
                Name = p.Name,
                DataType = p.DataType
            })
            .ToListAsync(ct);

        return list.ToDictionary(x => x.Id, x => x);
    }

    private async Task<IReadOnlyList<ActionRow>> LoadActionsAsync(Guid wellId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken ct)
        => await context.WellActions
            .Where(a => a.WellId == wellId && a.Timestamp >= fromUtc && a.Timestamp <= toUtc)
            .OrderBy(a => a.Timestamp)
            .Select(a => new ActionRow
            {
                Id = a.Id,
                Timestamp = a.Timestamp,
                Title = a.Title,
                Details = a.Details,
                Source = a.Source
            })
            .ToListAsync(ct);

    private async Task<IReadOnlyList<InsightSeries>> FetchSeriesAsync(
        string view,
        Guid wellId,
        Guid[] parameterIds,
        DateTimeOffset fromUtc,
        DateTimeOffset toUtc,
        IReadOnlyDictionary<Guid, ParameterMeta> parameterMap,
        CancellationToken ct)
    {
        using var conn = sqlFactory.Create();

        var sql = $"""
                  SELECT
                    time AS "Time",
                    well_id AS "WellId",
                    parameter_id AS "ParameterId",
                    avg_value AS "AvgValue",
                    min_value AS "MinValue",
                    max_value AS "MaxValue",
                    mode_value AS "ModeValue"
                  FROM {view}
                  WHERE well_id = @WellId
                    AND parameter_id = ANY(@ParameterIds)
                    AND time >= @From
                    AND time <= @To
                  ORDER BY time
                  """;

        var command = new CommandDefinition(sql, new
        {
            WellId = wellId,
            ParameterIds = parameterIds,
            From = fromUtc,
            To = toUtc
        }, cancellationToken: ct);

        var rows = (await conn.QueryAsync<MetricAggRow>(command)).AsList();

        var grouped = rows
            .GroupBy(r => r.ParameterId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.Time).ToList());

        var result = new List<InsightSeries>(parameterIds.Length);

        foreach (var pid in parameterIds)
        {
            if (!parameterMap.TryGetValue(pid, out var meta))
                continue;

            grouped.TryGetValue(pid, out var pointsRaw);
            pointsRaw ??= [];

            var aggregation = WellInsightRules.ResolveAggregation(meta.DataType);
            if (!WellInsightKnowledge.SupportsAggregation(meta.DataType, aggregation))
                aggregation = AggregationType.Mode;

            var points = pointsRaw
                .Select(r => new InsightPoint
                {
                    Timestamp = r.Time,
                    Value = FormatValue(meta.DataType, aggregation, r)
                })
                .ToList();

            result.Add(new InsightSeries
            {
                ParameterId = pid,
                Name = meta.Name,
                DataType = meta.DataType,
                Points = points,
                Aggregation = aggregation
            });
        }

        return result;
    }

    private static string FormatValue(ParameterDataType dataType, AggregationType aggregation, MetricAggRow row)
    {
        if (dataType == ParameterDataType.Categorical || aggregation == AggregationType.Mode)
            return row.ModeValue ?? string.Empty;

        var value = aggregation switch
        {
            AggregationType.Min => row.MinValue,
            AggregationType.Max => row.MaxValue,
            _ => row.AvgValue
        };

        return value?.ToString(CultureInfo.InvariantCulture) ?? "0";
    }

    private static IReadOnlyList<InsightKpi> BuildKpis(IReadOnlyList<InsightSeries> series)
    {
        var kpis = new List<InsightKpi>();

        foreach (var s in series)
        {
            if (s.Points.Count == 0)
                continue;

            var first = s.Points[0].Value;
            var last = s.Points[^1].Value;

            string? delta = null;

            if (s.DataType != ParameterDataType.Categorical
                && decimal.TryParse(first, NumberStyles.Any, CultureInfo.InvariantCulture, out var f)
                && decimal.TryParse(last, NumberStyles.Any, CultureInfo.InvariantCulture, out var l))
            {
                delta = (l - f).ToString(CultureInfo.InvariantCulture);
            }

            kpis.Add(new InsightKpi
            {
                Code = $"last:{s.ParameterId}",
                Label = $"{s.Name} (last)",
                Value = last,
                Delta = delta
            });
        }

        return kpis;
    }

    private async Task<WellInsightAiEnvelope> GenerateAiAsync(
        Guid wellId,
        DateTimeOffset fromUtc,
        DateTimeOffset toUtc,
        GroupingInterval interval,
        InsightPayload payload,
        CancellationToken ct)
    {
        var prompt = WellInsightPromptBuilder.Build(wellId, fromUtc, toUtc, interval, payload);
        var r = await ai.GenerateAsync(prompt, ct);

        var title = string.IsNullOrWhiteSpace(r.Title) ? $"Insight ({interval})" : r.Title.Trim();
        var summary = string.IsNullOrWhiteSpace(r.Summary) ? "Generated insight." : r.Summary.Trim();

        return new WellInsightAiEnvelope
        {
            Title = title,
            Summary = summary,
            Highlights = r.Highlights ?? [],
            Suspicions = r.Suspicions ?? [],
            RecommendedActions = r.RecommendedActions ?? []
        };
    }

    private static InsightEvent ToEvent(ActionRow a)
        => new()
        {
            Timestamp = a.Timestamp,
            Title = a.Title,
            Details = a.Details,
            Source = a.Source
        };

    private sealed class MetricAggRow
    {
        public DateTimeOffset Time { get; init; }
        public Guid WellId { get; init; }
        public Guid ParameterId { get; init; }
        public decimal? AvgValue { get; init; }
        public decimal? MinValue { get; init; }
        public decimal? MaxValue { get; init; }
        public string? ModeValue { get; init; }
    }

    private sealed class ParameterMeta
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public ParameterDataType DataType { get; init; }
    }

    private sealed class ActionRow
    {
        public Guid Id { get; init; }
        public DateTimeOffset Timestamp { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Details { get; init; } = string.Empty;
        public WellActionSource Source { get; init; }
    }

    private sealed class WellInsightAiEnvelope
    {
        public string Title { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
        public string[] Highlights { get; init; } = [];
        public string[] Suspicions { get; init; } = [];
        public string[] RecommendedActions { get; init; } = [];
    }
}
