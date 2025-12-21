using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.WellInsights.Common;
using WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;
using WellInsightEngine.Core.Features.WellInsights.GetWellInsight;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class WellInsightsController : ControllerBase
{
    [Authorize(Policies.ByEmail)]
    [HttpPost("[action]")]
    public async Task<ActionResult<WellInsightResponse>> GenerateWellInsight([FromBody] GenerateWellInsightRequest request, [FromServices] GenerateWellInsightFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(request, cancellation);
        return Ok(result);
    }
    
    [Authorize(Policies.ByEmail)]
    [HttpGet("[action]:{Slug}")]
    public async Task<ActionResult<WellInsightResponse>> GetWellInsight([FromRoute] GetWellInsightRequest request, [FromServices] GetWellInsightFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(request, cancellation);
        
        if (result is null)
            return NotFound();
        
        return Ok(result);
    }
}