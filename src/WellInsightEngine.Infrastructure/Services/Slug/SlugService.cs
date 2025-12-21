using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using WellInsightEngine.Core.Abstractions.Services.Slug;

namespace WellInsightEngine.Infrastructure.Services.Slug;

public sealed partial class SlugService : ISlugService
{

    public string Provide(string title, Guid id)
    {
        var titleSlug = Slugify(title);
        var suffix = id.ToString("N").ToLowerInvariant();
        return string.IsNullOrWhiteSpace(titleSlug) ? suffix : $"{titleSlug}-{suffix}";
    }

    public bool TryDecode(string slug, out Guid id)
    {
        id = Guid.Empty;

        if (string.IsNullOrWhiteSpace(slug))
            return false;

        var lastDash = slug.LastIndexOf('-');
        if (lastDash < 0 || lastDash == slug.Length - 1)
            return false;

        var suffix = slug[(lastDash + 1)..];

        return suffix.Length == 32 && Guid.TryParseExact(suffix, "N", out id);
    }

    private static string Slugify(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var s = input.Trim().ToLowerInvariant();

        s = TransliterateUaRuToLatin(s);

        s = s.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(s.Length);

        foreach (var ch in from ch in s let uc = CharUnicodeInfo.GetUnicodeCategory(ch) where uc != UnicodeCategory.NonSpacingMark select ch)
            sb.Append(ch);

        s = sb.ToString().Normalize(NormalizationForm.FormC);
        s = SlugRegex().Replace(s, "-");
        s = SlugRegexes.NonSlug().Replace(s, "");
        s = SlugRegexes.MultiDash().Replace(s, "-");
        s = s.Trim('-');

        return s;
    }

    private static string TransliterateUaRuToLatin(string s)
    {
        var sb = new StringBuilder(s.Length);

        foreach (var ch in s)
        {
            sb.Append(ch switch
            {
                'а' => "a", 'б' => "b", 'в' => "v", 'г' => "h", 'ґ' => "g", 'д' => "d",
                'е' => "e", 'ё' => "e", 'є' => "ye", 'ж' => "zh", 'з' => "z", 'и' => "y",
                'і' => "i", 'ї' => "yi", 'й' => "y", 'к' => "k", 'л' => "l", 'м' => "m",
                'н' => "n", 'о' => "o", 'п' => "p", 'р' => "r", 'с' => "s", 'т' => "t",
                'у' => "u", 'ф' => "f", 'х' => "kh", 'ц' => "ts", 'ч' => "ch", 'ш' => "sh",
                'щ' => "shch", 'ы' => "y", 'э' => "e", 'ю' => "yu", 'я' => "ya",
                'ъ' => "", 'ь' => "", '’' => "", '\'' => "", '№' => "",
                _ => ch.ToString()
            });
        }

        return sb.ToString();
    }

    [GeneratedRegex(@"[\s_]+")]
    private static partial Regex SlugRegex();
}