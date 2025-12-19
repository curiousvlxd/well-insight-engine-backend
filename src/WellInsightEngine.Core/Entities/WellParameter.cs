using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities;

public sealed class WellParameter
{
    public Guid Id { get; init; }
    public Guid WellId { get; set; }
    public Well Well { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public ParameterDataType DataType { get; set; }
}