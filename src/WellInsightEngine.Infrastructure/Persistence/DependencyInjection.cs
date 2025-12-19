using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WellInsightEngine.Infrastructure.Persistence.Options;

namespace WellInsightEngine.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();
        services.AddDbContext<ApplicationDbContext>((sp, o) =>
        {
            var db = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            o.UseNpgsql(db.Postgres.ConnectionString);
            o.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}