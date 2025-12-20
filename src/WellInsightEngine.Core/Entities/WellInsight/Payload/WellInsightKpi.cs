using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightKpi
{
    public required Guid ParameterId { get; init; }
    public required WellInsightMetricKind Kind { get; init; }
    public required string Name { get; init; } 
    public required string Value { get; init; }
    public string? Change { get; init; }   
    public static WellInsightKpi Create(Guid parameterId, WellInsightMetricKind kind, string name, string value, string? change)
        => new()
        {
            ParameterId = parameterId,
            Kind = kind,
            Name = name,
            Value = value,
            Change = change
        };
}