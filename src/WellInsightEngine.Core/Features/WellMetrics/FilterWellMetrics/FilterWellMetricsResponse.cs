namespace WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;


public sealed class FilterWellMetricsResponse
{
    public List<MetricSeriesResponse> Series { get; init; } = [];
}

public sealed class MetricSeriesResponse
{
    public Guid WellId { get; init; }
    public Guid ParameterId { get; init; }
    public string ParameterName { get; init; } = string.Empty;
    public List<MetricTickResponse> DateTicks { get; init; } = [];
}

public sealed class MetricTickResponse
{
    public DateTimeOffset Time { get; init; }
    public string Value { get; init; } = string.Empty;
}