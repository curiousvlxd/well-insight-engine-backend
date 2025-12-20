using System.Globalization;
using Dapper;
using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Abstractions.Services.Ai;
using WellInsightEngine.Core.Abstractions.Services.Slug;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight;
using WellInsightEngine.Core.Entities.WellInsight.Payload;
using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Extensions;
using WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;

public sealed class GenerateWellInsightFeature(
    ISqlConnectionFactory sqlFactory,
    ISlugService slugService,
    IApplicationDbContext context,
    IGoogleAiService ai)
{
    public async Task<GenerateWellInsightResponse> Handle(GenerateWellInsightRequest request, CancellationToken cancellation)
    {
        var fromUtc = request.From.ToUniversalTime();
        var toUtc = request.To.ToUniversalTime();
        var well = await LoadWellAsync(request.WellId, cancellation);
        var parameterMap = await LoadParametersAsync(request.ParameterIds, cancellation);
        var actions = await LoadActionsAsync(request.WellId, fromUtc, toUtc, cancellation);
        var interval = WellInsightRules.ChooseInterval(fromUtc, toUtc, request.MaxMetrics);
        var aggregate = WellInsightKnowledge.ResolveAggregate(interval);
        var series = await FetchSeriesAsync(aggregate, request.WellId, request.ParameterIds, fromUtc, toUtc, parameterMap, cancellation);
        var payload = BuildPayload(series);
        var aiEnvelope = await GenerateAiAsync(well, fromUtc, toUtc, interval, payload, actions, cancellation);
        var insight = WellInsight.Create(slugService, interval, well.Id, fromUtc, toUtc, aiEnvelope.Title, aiEnvelope.Summary, aiEnvelope.Highlights, aiEnvelope.Suspicions, aiEnvelope.RecommendedActions, payload);
        context.Add(insight);
        await context.SaveChangesAsync(cancellation);
        return GenerateWellInsightResponse.Create(insight);
    }

    private async Task<Well> LoadWellAsync(Guid wellId, CancellationToken ct)
        => await context.Wells
            .Where(w => w.Id == wellId)
            .Include(w => w.Asset)
            .FirstOrDefaultAsync(ct)
            ?? throw new InvalidOperationException($"Cannot find well: {wellId}");

    private async Task<IReadOnlyDictionary<Guid, Parameter>> LoadParametersAsync(IEnumerable<Guid> parameterIds, CancellationToken ct)
    {
        var list = await context.Parameters
            .Where(p => parameterIds.Contains(p.Id))
            .ToListAsync(ct);
        return list.ToDictionary(x => x.Id, x => x);
    }

    private async Task<IReadOnlyList<WellAction>> LoadActionsAsync(Guid wellId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken ct)
        => await context.WellActions
            .Where(a => a.WellId == wellId && a.Timestamp >= fromUtc && a.Timestamp <= toUtc)
            .OrderBy(a => a.Timestamp)
            .ToListAsync(ct);

    private async Task<IReadOnlyList<WellInsightParameter>> FetchSeriesAsync(string aggregate, Guid wellId, Guid[] parameterIds, DateTimeOffset fromUtc, DateTimeOffset toUtc, IReadOnlyDictionary<Guid, Parameter> parameterMap, CancellationToken cancellation)
    {
        using var conn = sqlFactory.Create();

        var sql = $"""
                  SELECT
                    well_id,
                    time,
                    parameter_id,
                    avg_value,
                    min_value,
                    max_value,
                    mode_value
                  FROM {aggregate}
                  WHERE well_id = @WellId
                    AND parameter_id = ANY(@ParameterIds)
                    AND time >= @From
                    AND time <= @To
                  ORDER BY time
                  """;

        var command = new CommandDefinition(sql, new { WellId = wellId, ParameterIds = parameterIds, From = fromUtc, To = toUtc }, cancellationToken: cancellation);
        var aggregations = (await conn.QueryAsync<WellMetricAggregation>(command)).AsList();
        var byParameter = aggregations
            .GroupBy(r => r.ParameterId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.Time).ToList());
        var result = new List<WellInsightParameter>(parameterIds.Length);

        foreach (var parameterId in parameterIds)
        {
            if (!parameterMap.TryGetValue(parameterId, out var meta))
                continue;

            byParameter.TryGetValue(parameterId, out var metrics);
            metrics ??= [];
            var requested = WellInsightKnowledge.DefaultAggregation(meta.DataType);
            var aggregation = WellInsightKnowledge.ResolveAggregation(meta.DataType, requested);
            var insightMetrics = metrics
                .Select(r => WellInsightMetric.Create(r.Time, WellInsightKnowledge.FormatValue(meta.DataType, aggregation, r)))
                .ToList();
            result.Add(WellInsightParameter.Create(parameterId, meta.Name, meta.DataType, insightMetrics, aggregation));
        }

        return result;
    }

    private static WellInsightPayload BuildPayload(IReadOnlyList<WellInsightParameter> parameters) => new()
    {
        Parameters = parameters,
        Kpis = BuildKpis(parameters)
    };


    private static List<WellInsightKpi> BuildKpis(
        IReadOnlyList<WellInsightParameter> parameters)
    {
        var result = new List<WellInsightKpi>();

        foreach (var p in parameters)
        {
            if (p.Metrics.Count == 0)
                continue;

            var first = p.Metrics[0].Value;
            var last = p.Metrics[^1].Value;
            string? change = null;

            if (p.DataType is not ParameterDataType.Categorical
                && decimal.TryParse(first, NumberStyles.Any, CultureInfo.InvariantCulture, out var f)
                && decimal.TryParse(last, NumberStyles.Any, CultureInfo.InvariantCulture, out var l))
                change = (l - f).ToString(CultureInfo.InvariantCulture);

            result.Add(WellInsightKpi.Create(p.ParameterId, WellInsightMetricKind.Last, p.Name, last, change));
        }

        return result;
    }

    private async Task<WellInsightAiEnvelope> GenerateAiAsync(Well well, DateTimeOffset fromUtc, DateTimeOffset toUtc, GroupingInterval interval, WellInsightPayload payload, IReadOnlyList<WellAction> actions, CancellationToken cancellation)
    {
        var prompt = WellInsightPromptBuilder.Build(well.Asset?.Name, well.Name, fromUtc, toUtc, interval, payload, actions);
        var aiResponse = await ai.GenerateAsync(prompt, cancellation);
        var envelope = GenerateWellInsightMapper.ToEnvelope(aiResponse);
        return envelope with
        {
            Title = Safe(envelope.Title) ?? BuildFallbackTitle(well, interval),
            Summary = Safe(envelope.Summary) ?? "Згенерований інсайт на основі агрегацій та подій."
        };
    }


    private static string BuildFallbackTitle(Well well, GroupingInterval interval)
    {
        var asset = string.IsNullOrWhiteSpace(well.Asset?.Name) ? null : well.Asset!.Name.Trim();
        var description = interval.GetDescription();
        return asset is null
            ? $"Інсайт: {well.Name} ({description})"
            : $"Інсайт: {well.Name} | Ассет {asset} ({description})";
    }

    private static string? Safe(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}