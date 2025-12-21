using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Features.Common.Pagination;

namespace WellInsightEngine.Core.Extensions;

public static class OffsetPaginationExtensions
{
    public static async Task<OffsetPagedList<T>> ToOffsetPagedListAsync<T>(
        this IQueryable<T> query,
        OffsetPagination pagination,
        CancellationToken ct = default)
    {
        var total = await query.CountAsync(ct);
        var items = await query
            .Skip(pagination.Offset)
            .Take(pagination.Limit)
            .ToListAsync(ct);
        return new OffsetPagedList<T>
        {
            Items = items,
            Total = total,
            Offset = pagination.Offset,
            Limit = pagination.Limit
        };
    }
    
    public static IEnumerable<ValidationResult> ValidatePagination(this OffsetPagination pagination, ValidationContext context)
    {
        var innerContext = new ValidationContext(pagination, context, context.Items);
        foreach (var r in pagination.Validate(innerContext))
            yield return r;
    }
}
