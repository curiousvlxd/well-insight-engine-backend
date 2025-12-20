namespace WellInsightEngine.Core.Abstractions.Services.Slug;

public interface ISlugService
{
    string Provide(string title, Guid id);
    bool TryDecode(string slug, out Guid id);
}