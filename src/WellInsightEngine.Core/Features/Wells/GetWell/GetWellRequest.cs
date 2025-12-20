using System.ComponentModel.DataAnnotations;

namespace WellInsightEngine.Core.Features.Wells.GetWell;

public sealed record GetWellRequest
{
    [Required] public Guid WellId { get; init; }
}