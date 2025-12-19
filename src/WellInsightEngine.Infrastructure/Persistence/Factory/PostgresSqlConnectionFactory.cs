using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Infrastructure.Persistence.Options;

namespace WellInsightEngine.Infrastructure.Persistence.Factory;

public sealed class PostgresSqlConnectionFactory(IOptions<DatabaseOptions> options) : ISqlConnectionFactory
{
    private readonly string _postgres = options.Value.Postgres.ConnectionString;
    private readonly string _timescale = options.Value.TimescaleDb.ConnectionString;

    public IDbConnection Create(SqlConnectionTarget target)
    {
        return target switch
        {
            SqlConnectionTarget.Postgres => new NpgsqlConnection(_postgres),
            SqlConnectionTarget.TimescaleDb => new NpgsqlConnection(_timescale),
            _ => throw new ArgumentOutOfRangeException(nameof(target))
        };
    }
}