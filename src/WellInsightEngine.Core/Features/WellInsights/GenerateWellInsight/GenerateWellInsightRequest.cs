using System.ComponentModel.DataAnnotations;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;

public sealed class GenerateWellInsightRequest : IValidatableObject
{
    [Required]
    public Guid WellId { get; init; }

    [Required, MinLength(1)]
    public Guid[] ParameterIds { get; init; } = [];

    [Required]
    public DateTimeOffset From { get; init; }

    [Required]
    public DateTimeOffset To { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (From >= To)
            yield return new ValidationResult("'From' must be earlier than 'To'", [nameof(From), nameof(To)]);
    }
}