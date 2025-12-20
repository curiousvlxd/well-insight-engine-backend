using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.Wells.GetWell;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class WellsController(GetWellFeature feature) : ControllerBase
{
    [HttpGet("[action]:{wellId:guid}")]
    [Authorize(Policies.ByEmail)]
    public async Task<ActionResult<GetWellResponse>> GetWell(Guid wellId, CancellationToken ct)
    {
        var result = await feature.Handle(wellId, ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}