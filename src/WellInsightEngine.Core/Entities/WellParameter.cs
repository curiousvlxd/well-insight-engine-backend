using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities;

public sealed class WellParameter
{
    public Guid Id { get; init; }

    public Guid WellId { get; set; }
    public Well Well { get; set; } = null!;

    public Guid ParameterId { get; set; }
    public Parameter? Parameter { get; set; }
}