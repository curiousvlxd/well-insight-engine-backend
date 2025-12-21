using System.ComponentModel.DataAnnotations;

namespace WellInsightEngine.Core.Features.Common.Pagination;

public sealed class OffsetPagination : IValidatableObject
{
    public int Offset { get; init; } = 0;
    public int Limit { get; init; } = 50;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Offset < 0)
        {
            yield return new ValidationResult(
                "'Offset' must be greater or equal to 0",
                [nameof(Offset)]
            );
        }

        if (Limit <= 0)
        {
            yield return new ValidationResult(
                "'Limit' must be greater than 0",
                [nameof(Limit)]
            );
        }

        if (Limit > 500)
        {
            yield return new ValidationResult(
                "'Limit' must be less or equal to 500",
                [nameof(Limit)]
            );
        }
    }
}