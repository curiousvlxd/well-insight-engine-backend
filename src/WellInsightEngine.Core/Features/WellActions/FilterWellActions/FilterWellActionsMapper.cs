using Riok.Mapperly.Abstractions;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Core.Features.WellActions.FilterWellActions;

[Mapper]
public static partial class FilterWellActionsMapper
{
    public static partial IQueryable<WellActionResponse> ProjectToResponse(this IQueryable<WellAction> q);
    private static partial WellActionResponse Map(WellAction action);
}