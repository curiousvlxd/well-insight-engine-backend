using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.WellActions.FilterWellActions;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class WellActionsController : ControllerBase
{
    [Authorize(Policies.ByEmail)]
    [HttpPost]
    public async Task<ActionResult<FilterWellActionsResponse>> FilterWellActions([FromBody] FilterWellActionsRequest request, [FromServices] FilterWellActionsFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(request, cancellation);
        return Ok(result);
    }
}