using Riok.Mapperly.Abstractions;
using WellInsightEngine.Core.Entities.WellInsight;

namespace WellInsightEngine.Core.Features.WellInsights.FilterWellInsights;

[Mapper]
public static partial class FilterWellInsightsMapper
{
    public static partial IQueryable<WellInsightItemResponse> ProjectToResponse(this IQueryable<WellInsight> q);
}