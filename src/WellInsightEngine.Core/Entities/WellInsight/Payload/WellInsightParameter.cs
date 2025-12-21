namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightParameter
{
    public required Guid WellId { get; init; }
    public required Guid ParameterId { get; init; }
    public required string ParameterName { get; init; }
    public required IReadOnlyList<WellInsightMetric> DateTicks { get; init; }
    
    public static WellInsightParameter Create(Guid wellId, Guid parameterId, string parameterName, IReadOnlyList<WellInsightMetric> dateTicks)
        => new()
        {
            WellId = wellId,
            ParameterId = parameterId,
            ParameterName = parameterName,
            DateTicks = dateTicks
        };
}