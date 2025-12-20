using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class WellMetricsController : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> FilterWellMetrics([FromBody] FilterWellMetricsRequest request, [FromServices] FilterWellMetricsFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(request, cancellation);
        return Ok(result);
    }
}