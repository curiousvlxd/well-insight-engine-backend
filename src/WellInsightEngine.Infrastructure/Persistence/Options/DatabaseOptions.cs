namespace WellInsightEngine.Infrastructure.Persistence.Options;

public sealed record DatabaseOptions
{   
    public required PostgresOptions Postgres { get; init; }
    public required TimescaleDbOptions TimescaleDb { get; init; }

    public sealed record PostgresOptions
    {
        public required string ConnectionString { get; init; }
    }
    
    public sealed record TimescaleDbOptions
    {
        public required string ConnectionString { get; init; }
    }
}