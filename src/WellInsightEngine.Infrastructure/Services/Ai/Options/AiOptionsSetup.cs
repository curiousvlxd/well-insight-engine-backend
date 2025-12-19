using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WellInsightEngine.Infrastructure.Persistence.Options;

namespace WellInsightEngine.Infrastructure.Services.Ai.Options;

public sealed class AiOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<AiOptions>
{
    public void Configure(AiOptions options)
    {
        configuration.GetSection("Ai").Bind(options);
    }
}