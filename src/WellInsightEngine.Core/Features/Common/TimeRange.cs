using System.ComponentModel.DataAnnotations;

namespace WellInsightEngine.Core.Features.Common;

public sealed record TimeRange : IValidatableObject
{
    [Required]
    public DateTimeOffset From { get; init; }

    [Required]
    public DateTimeOffset To { get; init; }

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