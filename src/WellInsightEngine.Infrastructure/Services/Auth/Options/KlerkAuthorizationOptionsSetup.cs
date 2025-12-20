using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WellInsightEngine.Infrastructure.Services.Auth.Options;

public sealed class KlerkAuthorizationOptionsSetup(IConfiguration configuration) : IConfigureOptions<KlerkAuthorizationOptions>
{
    public void Configure(KlerkAuthorizationOptions options)
        => configuration.GetSection("Klerk").Bind(options);
}