using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Infrastructure.Persistence.Options;

namespace WellInsightEngine.Infrastructure.Extensions;

public static class MigrationExtension
{
    public static async Task MigrateIfNeededAsync(this IHost host)
    {   
        using var scope = host.Services.CreateScope();
        var databaseOptions = scope.ServiceProvider.GetService<IOptions<DatabaseOptions>>()?.Value;
        var migrate = databaseOptions?.MigrateOnStartup ?? false;

        if (migrate)
        {
            var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}