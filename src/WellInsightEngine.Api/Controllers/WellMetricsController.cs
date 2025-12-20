using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Extensions;
using WellInsightEngine.Core.Features.WellMetrics;
using WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class WellMetricsController : ControllerBase
{   
    [Authorize(Policies.ByEmail)]
    [HttpPost]
    public async Task<ActionResult<FilterWellMetricsResponse>> FilterWellMetrics([FromBody] FilterWellMetricsRequest request, [FromServices] FilterWellMetricsFeature feature, CancellationToken cancellation)
    {
        var result = await feature.Handle(request, cancellation);
        return Ok(result);
    }

    [Authorize(Policies.ByEmail)]
    [HttpGet]
    public ActionResult<string[]> GetGroupingIntervals()
    {
        var intervals = Enum.GetValues<GroupingInterval>()
            .Select(v => v.GetDescription() ?? v.ToString())
            .ToArray();
        return Ok(intervals);
    }
}