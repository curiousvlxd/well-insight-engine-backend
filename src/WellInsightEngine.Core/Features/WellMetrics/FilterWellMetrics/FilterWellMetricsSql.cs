using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;

internal static class FilterWellMetricsSql
{
    public static string Build(FilterWellMetricsRequest request)
    {
        if (request.Aggregation is null)
        {
            return """
                   SELECT time, well_id, parameter_id, value
                   FROM well_metrics
                   WHERE well_id = @WellId
                     AND parameter_id = ANY(@ParameterIds)
                     AND time BETWEEN @From AND @To
                   ORDER BY time
                   """;
        }

        if (!WellMetricsAggregations.Map.TryGetValue(request.Aggregation.Interval, out var view))
            throw new InvalidOperationException($"Unsupported interval: {request.Aggregation.Interval}");

        var expression = request.Aggregation.Type switch
        {
            AggregationType.Avg => "avg_value",
            AggregationType.Min => "min_value",
            AggregationType.Max => "max_value",
            AggregationType.Mode => "mode_value",
            _ => throw new InvalidOperationException($"Unsupported aggregation type: {request.Aggregation.Type}")
        };

        return $"""
                SELECT time, well_id, parameter_id, {expression} AS value
                FROM {view}
                WHERE well_id = @WellId
                  AND parameter_id = ANY(@ParameterIds)
                  AND time BETWEEN @From AND @To
                ORDER BY time
                """;
    }
}