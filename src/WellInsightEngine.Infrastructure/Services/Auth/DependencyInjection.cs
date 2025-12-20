using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WellInsightEngine.Infrastructure.Services.Auth.Options;

namespace WellInsightEngine.Infrastructure.Services.Auth;

public static class AuthenticationDependencyInjection
{
    public static IServiceCollection AddKlerk(this IServiceCollection services)
    {
        services.ConfigureOptions<KlerkAuthorizationOptionsSetup>();
        using var sp = services.BuildServiceProvider();
        var options = sp.GetRequiredService<IOptions<KlerkAuthorizationOptions>>().Value;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.Authority = options.Issuer;
                o.RequireHttpsMetadata = true;
                o.TokenValidationParameters.ValidateIssuer = true;
                o.TokenValidationParameters.ValidateAudience = false;
                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        if (ctx.Principal?.Identity is not ClaimsIdentity identity)
                            return Task.CompletedTask;

                        var email = ctx.Principal.FindFirst("email")?.Value
                                    ?? ctx.Principal.FindFirst(ClaimTypes.Email)?.Value
                                    ?? ctx.Principal.Claims.FirstOrDefault(c => c.Type.EndsWith("/emailaddress", StringComparison.OrdinalIgnoreCase))?.Value;

                        if (!string.IsNullOrWhiteSpace(email) && identity.FindFirst(ClaimTypes.Email) is null)
                            identity.AddClaim(new Claim(ClaimTypes.Email, email));

                        if (options.AuthorizedParties.Length <= 0) return Task.CompletedTask;
                        
                        var azp = ctx.Principal.FindFirst("azp")?.Value;

                        if (string.IsNullOrWhiteSpace(azp) || !options.AuthorizedParties.Contains(azp, StringComparer.OrdinalIgnoreCase))
                            ctx.Fail("Unauthorized client (azp).");
                       
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddSingleton<IAuthorizationHandler, AllowedEmailHandler>();
        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.ByEmail, policy =>
                policy.RequireAuthenticatedUser()
                    .AddRequirements(new AllowedEmailRequirement()));
        return services;
    }
}