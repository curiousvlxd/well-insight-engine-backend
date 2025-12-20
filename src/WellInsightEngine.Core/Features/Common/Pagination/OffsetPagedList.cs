namespace WellInsightEngine.Core.Features.Common.Pagination;

public sealed class OffsetPagedList<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required int Total { get; init; }
    public required int Offset { get; init; }
    public required int Limit { get; init; }
    public bool HasNext => Offset + Items.Count < Total;
    public bool HasPrevious => Offset > 0;
}