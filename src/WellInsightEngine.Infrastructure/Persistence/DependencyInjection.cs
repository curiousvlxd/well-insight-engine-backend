using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Infrastructure.Persistence.Factory;
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
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddSingleton<ISqlConnectionFactory, PostgresSqlConnectionFactory>();
        return services;
    }
}