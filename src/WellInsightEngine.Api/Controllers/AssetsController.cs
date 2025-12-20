using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.Assets.GetAssets;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class AssetsController : ControllerBase
{
    [HttpGet]
    [Authorize(Policies.ByEmail)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetAssetsResponse>> GetAssets([FromServices] GetAssetsFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(cancellation);
        return Ok(result);
    }
}