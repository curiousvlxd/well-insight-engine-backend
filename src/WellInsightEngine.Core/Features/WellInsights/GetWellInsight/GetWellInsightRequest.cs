using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WellInsightEngine.Core.Abstractions.Services.Slug;

namespace WellInsightEngine.Core.Features.WellInsights.GetWellInsight;

public sealed partial record GetWellInsightRequest : IValidatableObject
{
    [Required]
    public string Slug { get; init; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Slug))
        {
            yield return new ValidationResult("Slug is required.", [nameof(Slug)]);
            yield break;
        }

        var s = Slug.Trim();

        if (SlugRegexes.NonSlug().IsMatch(s))
            yield return new ValidationResult("Slug contains invalid characters.", [nameof(Slug)]);

        if (SlugRegexes.MultiDash().IsMatch(s))
            yield return new ValidationResult("Slug must not contain consecutive dashes.", [nameof(Slug)]);

        if (s.StartsWith('-') || s.EndsWith('-'))
            yield return new ValidationResult(
                "Slug must not start or end with a dash.",
                [nameof(Slug)]);

    }
}