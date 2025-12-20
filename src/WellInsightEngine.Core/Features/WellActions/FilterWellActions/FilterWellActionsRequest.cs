using System.ComponentModel.DataAnnotations;
using WellInsightEngine.Core.Features.Common.Pagination;

namespace WellInsightEngine.Core.Features.WellActions.FilterWellActions;

public sealed class FilterWellActionsRequest : IValidatableObject
{
    [Required]
    public Guid WellId { get; init; }

    [Required]
    public DateTimeOffset From { get; init; }

    [Required]
    public DateTimeOffset To { get; init; }

    public OffsetPagination Pagination { get; init; } = new();
    public DateTimeOffset FromUtc => From.ToUniversalTime();
    public DateTimeOffset ToUtc => To.ToUniversalTime();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (From >= To)
        {
            yield return new ValidationResult(
                "'From' must be earlier than 'To'",
                [nameof(From), nameof(To)]
            );
        }

        if (Pagination.Limit <= 0)
        {
            yield return new ValidationResult(
                "'Limit' must be greater than 0",
                [nameof(Pagination.Limit)]
            );
        }

        if (Pagination.Offset < 0)
        {
            yield return new ValidationResult(
                "'Offset' must be greater or equal to 0",
                [nameof(Pagination.Offset)]
            );
        }
    }
}