using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WellInsightEngine.Infrastructure.Persistence.Options;

public sealed class DatabaseOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<DatabaseOptions>
{
    public void Configure(DatabaseOptions options)
    {
        configuration.GetSection("Database").Bind(options);
    }
}