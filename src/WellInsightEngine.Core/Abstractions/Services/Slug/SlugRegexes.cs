using System.Text.RegularExpressions;

namespace WellInsightEngine.Core.Abstractions.Services.Slug;

public static partial class SlugRegexes
{
    [GeneratedRegex("[^a-z0-9-]", RegexOptions.Compiled)]
    public static partial Regex NonSlug();
    
    [GeneratedRegex("-{2,}", RegexOptions.Compiled)]
    public static partial Regex MultiDash();
}