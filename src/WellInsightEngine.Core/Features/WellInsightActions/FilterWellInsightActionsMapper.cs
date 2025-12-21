using Riok.Mapperly.Abstractions;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Features.WellActions.FilterWellActions;

namespace WellInsightEngine.Core.Features.WellInsightActions;

[Mapper]
public static partial class FilterWellInsightActionsMapper
{
    public static partial IQueryable<WellActionResponse> ProjectToResponse(this IQueryable<WellAction> q);
    private static partial WellActionResponse Map(WellAction action);
}