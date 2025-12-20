using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WellInsightEngine.Core.Features.Assets.GetAssets;
using WellInsightEngine.Infrastructure.Services.Auth.Options;

namespace WellInsightEngine.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<GetAssetsResponse> GetAllowedEmails([FromServices] IOptions<KlerkAuthorizationOptions> options, CancellationToken cancellation)
    {
        var result = options.Value.AllowedEmails;
        return Ok(result);
    }
}