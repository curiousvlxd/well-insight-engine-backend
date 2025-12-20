using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Features.Wells.GetWell;

public sealed record GetWellResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ExternalId { get; init; } = string.Empty;
    public Guid AssetId { get; init; }
    public string AssetName { get; init; } = string.Empty;
    public List<ParameterResponse> Parameters { get; init; } = [];
}

public sealed record ParameterResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ParameterDataType DataType { get; init; }
}