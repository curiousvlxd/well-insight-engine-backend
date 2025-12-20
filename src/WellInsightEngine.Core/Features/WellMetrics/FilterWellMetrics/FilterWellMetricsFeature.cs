using Dapper;
using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;

public sealed class FilterWellMetricsFeature(ISqlConnectionFactory factory, IApplicationDbContext context)
{
    public async Task<FilterWellMetricsResponse> Handle(FilterWellMetricsRequest request, CancellationToken ct)
    {
        var sql = FilterWellMetricsSql.Build(request);
        using var conn = factory.Create();
        var command = new CommandDefinition(sql, request.ToSqlParams(), cancellationToken: ct);
        var rows = (await conn.QueryAsync<WellMetric>(command)).AsList();

        if (rows.Count == 0)
            return FilterWellMetricsMapper.Map(rows, new Dictionary<Guid, string>());

        var parameterIds = rows.Select(x => x.ParameterId).Distinct();
        var parameterNames = await context.Parameters
            .Where(p => parameterIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Name })
            .ToDictionaryAsync(x => x.Id, x => x.Name, ct);
        return FilterWellMetricsMapper.Map(rows, parameterNames);
    }
}