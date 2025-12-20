using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.GetAssets;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AssetsController(GetAssetsFeature feature) : ControllerBase
{
    [HttpGet]
    [Authorize(Policies.ByEmail)]
    public async Task<ActionResult<GetAssetsResponse>> GetAssets(CancellationToken ct)
    {
        var result = await feature.Handle(ct);
        return Ok(result);
    }
}