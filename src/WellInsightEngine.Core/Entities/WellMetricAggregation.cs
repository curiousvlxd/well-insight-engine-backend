using System.ComponentModel.DataAnnotations.Schema;

namespace WellInsightEngine.Core.Entities;

public sealed class WellMetricAggregation
{
    [Column("time")]
    public DateTimeOffset Time { get; init; }
    [Column("well_id")]
    public Guid WellId { get; init; }
    [Column("parameter_id")]
    public Guid ParameterId { get; init; }
    [Column("avg_value")]
    public decimal AvgValue { get; init; }
    [Column("min_value")]
    public decimal MinValue { get; init; }
    [Column("max_value")]
    public decimal MaxValue { get; init; }
    [Column("mode_value")]
    public string? ModeValue { get; init; }
}