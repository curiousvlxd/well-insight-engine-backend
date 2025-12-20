using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WellInsightEngine.Core.Converters;
using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;

public class FilterWellMetricsRequest : IValidatableObject
{
    [Required]
    public Guid WellId { get; init; }
    
    [Required]
    [MinLength(1, ErrorMessage = "At least one parameter must be specified.")]
    public Guid[] ParameterIds { get; init; } = [];
    
    [Required]
    public DateTimeOffset From { get; init; }
    
    [Required]
    public DateTimeOffset To { get; init; }
    
    public Aggregation? Aggregation { get; init; }
    
    internal object ToSqlParams() => new
    {
        WellId,
        ParameterIds,
        From = From.ToUniversalTime(),
        To = To.ToUniversalTime()
    };
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (From >= To)
        {
            yield return new ValidationResult(
                "'From' must be earlier than 'To'",
                [nameof(From), nameof(To)]
            );
        }
    }
}

public sealed class Aggregation
{       
    [JsonConverter(typeof(DescriptionEnumJsonConverter<GroupingInterval>))]
    public GroupingInterval Interval { get; init; }
    public AggregationType Type { get; init; }
}