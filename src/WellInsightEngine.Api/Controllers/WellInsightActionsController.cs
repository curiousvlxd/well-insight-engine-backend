using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.WellInsightActions;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class WellInsightActionsController : ControllerBase
{
    [Authorize(Policies.ByEmail)]
    [HttpPost]
    public async Task<ActionResult<FilterWellInsightActionsResponse>> FilterWellInsightActions([FromBody] FilterWellInsightActionsRequest request, [FromServices] FilterWellInsightActionsFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(request, cancellation);
        return Ok(result);
    }
}