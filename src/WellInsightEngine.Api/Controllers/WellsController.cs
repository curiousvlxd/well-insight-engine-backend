using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WellInsightEngine.Core.Features.Wells.GetWell;
using WellInsightEngine.Infrastructure.Services.Auth;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class WellsController(GetWellFeature feature) : ControllerBase
{
    [HttpGet("[action]:{WellId:guid}")]
    [Authorize(Policies.ByEmail)]
    public async Task<ActionResult<GetWellResponse>> GetWell([FromRoute] GetWellRequest request, CancellationToken cancellation)
    {
        var result = await feature.Handle(request, cancellation);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}