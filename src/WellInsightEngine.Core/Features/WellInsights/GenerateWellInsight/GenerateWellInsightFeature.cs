using System.Globalization;
using Dapper;
using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Abstractions.Services.Slug;
using WellInsightEngine.Core.Abstractions.Services.WellInsightsAi;
using WellInsightEngine.Core.Abstractions.Services.WellInsightsAi.Contracts;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight;
using WellInsightEngine.Core.Entities.WellInsight.Payload;
using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Features.WellInsights.Common;
using WellInsightEngine.Core.Services.WellInsightsAi;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;

public sealed class GenerateWellInsightFeature(ISqlConnectionFactory sqlFactory, ISlugService slugService, IApplicationDbContext context, IWellInsightsAiService wellInsightsAi)
{
    public async Task<WellInsightResponse> Handle(GenerateWellInsightRequest request, CancellationToken cancellation)
    {
        var fromUtc = request.From.ToUniversalTime();
        var toUtc = request.To.ToUniversalTime();

        var well = await LoadWellAsync(request.WellId, cancellation);
        var parameterMap = await LoadParametersAsync(request.ParameterIds, cancellation);
        var actions = await LoadActionsAsync(request.WellId, fromUtc, toUtc, cancellation);

        var plan = wellInsightsAi.ResolvePlan(fromUtc, toUtc);
        var series = await FetchSeriesAsync(plan, request.WellId, request.ParameterIds, fromUtc, toUtc, parameterMap, cancellation);
        var payload = WellInsightPayload.Create(series);

        var aiRequest = GenerateWellInsightAiRequest.Create(well, fromUtc, toUtc, plan.GroupingInterval, payload, actions);
        var ai = await wellInsightsAi.GenerateAsync(aiRequest, cancellation);
        var insight = WellInsight.Create(slugService, plan.GroupingInterval, well.Id, fromUtc, toUtc, ai.Envelope.Title, ai.Envelope.Summary, ai.Envelope.Highlights, ai.Envelope.Suspicions, ai.Envelope.RecommendedActions, ai.Payload);
        context.Add(insight);
        var links = ai.Actions
            .Select(a => WellInsightAction.Create(insight.Id, a.Id))
            .ToList();
        context.AddRange(links);
        await context.SaveChangesAsync(cancellation);
        return WellInsightResponseMapper.Map(insight);
    }

    private async Task<Well> LoadWellAsync(Guid wellId, CancellationToken ct)
        => await context.Wells
            .Where(w => w.Id == wellId)
            .Include(w => w.Asset)
            .FirstOrDefaultAsync(ct)
            ?? throw new InvalidOperationException($"Cannot find well: {wellId}");

    private async Task<IReadOnlyDictionary<Guid, Parameter>> LoadParametersAsync(IEnumerable<Guid> parameterIds, CancellationToken ct)
    {
        var parameters = await context.Parameters
            .Where(p => parameterIds.Contains(p.Id))
            .ToListAsync(ct);

        return parameters.ToDictionary(x => x.Id, x => x);
    }

    private async Task<IReadOnlyList<WellAction>> LoadActionsAsync(Guid wellId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken ct)
        => await context.WellActions
            .Where(a => a.WellId == wellId && a.Timestamp >= fromUtc && a.Timestamp <= toUtc)
            .OrderBy(a => a.Timestamp)
            .ToListAsync(ct);

    private async Task<IReadOnlyList<WellInsightAggregation>> FetchSeriesAsync(WellMetricAggregationPlan plan, Guid wellId, Guid[] parameterIds, DateTimeOffset fromUtc, DateTimeOffset toUtc, IReadOnlyDictionary<Guid, Parameter> parameterMap, CancellationToken cancellation)
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
                  FROM {plan.View}
                  WHERE well_id = @WellId
                    AND parameter_id = ANY(@ParameterIds)
                    AND time >= @From
                    AND time <= @To
                  ORDER BY time
                  """;

        var command = new CommandDefinition(sql, new { WellId = wellId, ParameterIds = parameterIds, From = fromUtc, To = toUtc }, cancellationToken: cancellation);
        var rows = (await conn.QueryAsync<WellMetricAggregation>(command)).AsList();

        var byParameter = rows
            .GroupBy(r => r.ParameterId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.Time).ToList());

        var bag = new List<(ParameterDataType DataType, AggregationType Agg, WellInsightParameter Param)>();

        foreach (var parameterId in parameterIds)
        {
            if (!parameterMap.TryGetValue(parameterId, out var meta))
                continue;

            byParameter.TryGetValue(parameterId, out var metrics);
            metrics ??= [];

            var aggs = WellInsightKnowledge.ResolveAggregationTypes(meta.DataType, plan.Aggregations);

            bag.AddRange(from agg in aggs
                let ticks = agg switch
                {
                    AggregationType.Min => metrics.Select(r => WellInsightMetric.Create(r.Time, r.MinValue.ToString(CultureInfo.InvariantCulture))).ToList(),
                    AggregationType.Max => metrics.Select(r => WellInsightMetric.Create(r.Time, r.MaxValue.ToString(CultureInfo.InvariantCulture))).ToList(),
                    AggregationType.Mode => metrics.Select(r => WellInsightMetric.Create(r.Time, r.ModeValue ?? string.Empty)).ToList(),
                    _ => metrics.Select(r => WellInsightMetric.Create(r.Time, r.AvgValue.ToString(CultureInfo.InvariantCulture))).ToList()
                }
                where agg is not AggregationType.Mode || !ticks.All(x => string.IsNullOrWhiteSpace(x.Value))
                select (meta.DataType, agg, WellInsightParameter.Create(wellId, parameterId, meta.Name, ticks)));
        }

        return bag
            .GroupBy(x => new { x.DataType, x.Agg })
            .Select(g => WellInsightAggregation.Create(
                g.Key.DataType,
                g.Key.Agg,
                g.Select(x => x.Param).ToList()))
            .ToList();
    }
}
