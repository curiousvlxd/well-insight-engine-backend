using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities;

public sealed class Parameter
{
    public Guid Id { get; init; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ParameterDataType DataType { get; set; }
    public List<WellParameter> Wells { get; } = [];
}