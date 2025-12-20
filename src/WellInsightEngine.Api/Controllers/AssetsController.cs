using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.GetAssets;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AssetsController : ControllerBase
{
    [HttpGet]
    [Authorize(Policies.ByEmail)]
    public async Task<ActionResult<GetAssetsResponse>> GetAssets([FromServices] GetAssetsFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(cancellation);
        return Ok(result);
    }
}