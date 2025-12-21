using System.ComponentModel.DataAnnotations;
using WellInsightEngine.Core.Extensions;
using WellInsightEngine.Core.Features.Common.Pagination;

namespace WellInsightEngine.Core.Features.WellInsights.FilterWellInsights;

public sealed record FilterWellInsightsRequest : IValidatableObject
{
    public WellInsightFilter? Filter { get; init; }

    public OffsetPagination Pagination { get; init; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var r in Pagination.ValidatePagination(validationContext))
            yield return r;

        if (Filter is null)
            yield break;

        foreach (var r in Filter.Validate(validationContext))
            yield return r;
    }
}

public sealed record WellInsightFilter : IValidatableObject
{
    [Required]
    public Guid WellId { get; init; }

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