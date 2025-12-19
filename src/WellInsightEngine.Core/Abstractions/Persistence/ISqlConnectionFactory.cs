using System.Data;

namespace WellInsightEngine.Core.Abstractions.Persistence;

public interface ISqlConnectionFactory
{
    IDbConnection Create(SqlConnectionTarget connection = SqlConnectionTarget.TimescaleDb);
}

public enum SqlConnectionTarget
{
    TimescaleDb = 1,
    Postgres = 2
}