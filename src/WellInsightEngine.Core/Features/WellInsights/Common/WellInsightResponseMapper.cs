using Riok.Mapperly.Abstractions;
using WellInsightEngine.Core.Entities.WellInsight;

namespace WellInsightEngine.Core.Features.WellInsights.Common;

[Mapper]
public static partial class WellInsightResponseMapper
{
    public static partial IQueryable<WellInsightResponse> ProjectToResponse(this IQueryable<WellInsight> q);
    public static partial WellInsightResponse Map(WellInsight insight);
}