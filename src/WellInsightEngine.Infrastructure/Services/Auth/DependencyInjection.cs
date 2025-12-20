using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WellInsightEngine.Infrastructure.Services.Auth;
using WellInsightEngine.Infrastructure.Services.Auth.Options;

public static class AuthenticationDependencyInjection
{
    public static IServiceCollection AddKlerk(this IServiceCollection services)
    {
        services.ConfigureOptions<KlerkAuthorizationOptionsSetup>();

        using var sp = services.BuildServiceProvider();
        var klerkOptions = sp.GetRequiredService<IOptions<KlerkAuthorizationOptions>>().Value;
        var auth = services.AddAuthorizationBuilder();

        if (klerkOptions.Disabled)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            auth.AddPolicy(Policies.ByEmail, p => p.RequireAssertion(_ => true));
            auth.SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAssertion(_ => true)
                .Build());
            return services;
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.Authority = klerkOptions.Issuer;
                o.RequireHttpsMetadata = true;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false
                };

                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        if (ctx.Principal?.Identity is not ClaimsIdentity identity)
                            return Task.CompletedTask;

                        var email =
                            ctx.Principal.FindFirst("email")?.Value ??
                            ctx.Principal.FindFirst(ClaimTypes.Email)?.Value ??
                            ctx.Principal.Claims.FirstOrDefault(c =>
                                c.Type.EndsWith("/emailaddress", StringComparison.OrdinalIgnoreCase)
                            )?.Value;

                        if (!string.IsNullOrWhiteSpace(email) &&
                            identity.FindFirst(ClaimTypes.Email) is null)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Email, email));
                        }

                        if (klerkOptions.AuthorizedParties.Length == 0)
                            return Task.CompletedTask;

                        var azp = ctx.Principal.FindFirst("azp")?.Value;

                        if (string.IsNullOrWhiteSpace(azp) ||
                            !klerkOptions.AuthorizedParties.Contains(azp, StringComparer.OrdinalIgnoreCase))
                        {
                            ctx.Fail("Unauthorized client (azp)");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddSingleton<IAuthorizationHandler, AllowedEmailHandler>();
        auth.AddPolicy(Policies.ByEmail, policy =>
            policy.RequireAuthenticatedUser()
                .AddRequirements(new AllowedEmailRequirement()));
        return services;
    }
}
