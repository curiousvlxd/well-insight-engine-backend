using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.GetAssets;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AssetsController(GetAssetsFeature feature) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAssetsResponse>> GetAssets(CancellationToken ct)
    {
        var result = await feature.Handle(ct);
        return Ok(result);
    }
}