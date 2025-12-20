using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class WellInsightsController : ControllerBase
{
    [Authorize(Policies.ByEmail)]
    [HttpPost]
    public async Task<ActionResult<GenerateWellInsightResponse>> GenerateWellInsight([FromBody] GenerateWellInsightRequest request, [FromServices] GenerateWellInsightFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(request, cancellation);
        return Ok(result);
    }
}