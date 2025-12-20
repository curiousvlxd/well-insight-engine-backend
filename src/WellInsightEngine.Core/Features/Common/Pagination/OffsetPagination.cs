namespace WellInsightEngine.Core.Features.Common.Pagination;

public sealed class OffsetPagination
{
    public int Offset { get; init; } = 0;
    public int Limit { get; init; } = 50;
}