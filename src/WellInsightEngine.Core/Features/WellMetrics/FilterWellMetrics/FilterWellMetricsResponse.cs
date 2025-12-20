namespace WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;

public class FilterWellMetricsResponse
{
    public List<MetricPointResponse> Metrics { get; init; } = [];
}

public sealed class MetricPointResponse
{
    public DateTimeOffset Time { get; init; }
    public Guid WellId { get; init; }
    public Guid ParameterId { get; init; }
    public string Value { get; init; } = string.Empty;
}