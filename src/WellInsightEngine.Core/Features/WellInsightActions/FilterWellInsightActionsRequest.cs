using System.ComponentModel.DataAnnotations;
using WellInsightEngine.Core.Extensions;
using WellInsightEngine.Core.Features.Common;
using WellInsightEngine.Core.Features.Common.Pagination;

namespace WellInsightEngine.Core.Features.WellInsightActions;

public sealed class FilterWellInsightActionsRequest : IValidatableObject
{
    [Required]
    public Guid InsightId { get; init; }

    public TimeRange? Range { get; init; }

    public OffsetPagination Pagination { get; init; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Range is not null)
        {
            foreach (var r in Range.Validate(validationContext))
                yield return r;
        }

        foreach (var r in Pagination.ValidatePagination(validationContext))
            yield return r;
    }
}