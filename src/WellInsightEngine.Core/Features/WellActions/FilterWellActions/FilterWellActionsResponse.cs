using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Features.Common.Pagination;

namespace WellInsightEngine.Core.Features.WellActions.FilterWellActions;

public sealed class FilterWellActionsResponse
{
    public required OffsetPagedList<WellActionResponse> Actions { get; init; }

    public static FilterWellActionsResponse Create(OffsetPagedList<WellActionResponse> actions)
    {
        return new FilterWellActionsResponse
        {
            Actions = actions
        };
    }
}

public sealed class WellActionResponse
{
    public Guid Id { get; init; }
    public Guid WellId { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Details { get; init; } = string.Empty;
    public WellActionSource Source { get; init; }
}