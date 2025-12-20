using Dapper;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;

public sealed class FilterWellMetricsFeature(ISqlConnectionFactory factory)
{
    public async Task<FilterWellMetricsResponse> Handle(FilterWellMetricsRequest request, CancellationToken ct)
    {
        var sql = FilterWellMetricsSql.Build(request);
        using var conn = factory.Create();
        var command = new CommandDefinition(sql, request.ToSqlParams(), cancellationToken: ct);
        var rows = (await conn.QueryAsync<WellMetric>(command)).AsList();
        return FilterWellMetricsMapper.Map(rows);
    }
}