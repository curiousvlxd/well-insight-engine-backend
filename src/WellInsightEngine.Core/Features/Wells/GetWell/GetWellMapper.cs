using Riok.Mapperly.Abstractions;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Core.Features.Wells.GetWell;

[Mapper]
public static partial class GetWellMapper
{
    public static partial IQueryable<GetWellResponse> ProjectToResponse(this IQueryable<Well> q);

    [MapProperty(nameof(Well.Asset.Name), nameof(GetWellResponse.AssetName))]
    private static partial GetWellResponse Map(Well well);

    private static partial ParameterResponse Map(Parameter parameter);
    private static List<ParameterResponse> MapParameters(List<WellParameter> parameters)
        => parameters
            .Where(p => p.Parameter != null)
            .Select(p => Map(p.Parameter!))
            .ToList();
}