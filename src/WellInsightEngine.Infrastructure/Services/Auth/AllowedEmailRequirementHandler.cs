using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using WellInsightEngine.Infrastructure.Services.Auth.Options;

namespace WellInsightEngine.Infrastructure.Services.Auth;

public sealed class AllowedEmailRequirement : IAuthorizationRequirement;

public sealed class AllowedEmailHandler(IOptionsMonitor<KlerkAuthorizationOptions> options) : AuthorizationHandler<AllowedEmailRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowedEmailRequirement requirement)
    {       
        var klerkOptions = options.CurrentValue;

        if (klerkOptions.Disabled)
        {   
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        var email = context.User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
            return Task.CompletedTask;

        if (klerkOptions.AllowedEmails.Length == 0 || klerkOptions.AllowedEmails.Contains(email, StringComparer.OrdinalIgnoreCase))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}