using WellInsightEngine.Core.Features.Common.Pagination;
using WellInsightEngine.Core.Features.WellActions.FilterWellActions;

namespace WellInsightEngine.Core.Features.WellInsightActions;

public sealed class FilterWellInsightActionsResponse
{
    public required OffsetPagedList<WellActionResponse> Actions { get; init; }

    public static FilterWellInsightActionsResponse Create(OffsetPagedList<WellActionResponse> actions)
        => new()
        {
            Actions = actions
        };
}