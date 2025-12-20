using System.ComponentModel.DataAnnotations.Schema;

namespace WellInsightEngine.Core.Entities;

[Table("well_metrics")]
public sealed class WellMetric
{
    [Column("time")]
    public DateTimeOffset Time { get; init; }

    [Column("well_id")]
    public Guid WellId { get; init; }

    [Column("parameter_id")]
    public Guid ParameterId { get; init; }

    [Column("value")]
    public string Value { get; init; } = string.Empty;
}