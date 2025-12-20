namespace WellInsightEngine.Infrastructure.Services.Ai.Options;

public sealed record AiOptions
{
    public string ProjectId { get; init; }
    public string Location { get; init; }
    public string Model { get; init; } = "gemini-2.5-flash-lite";
    public double Temperature { get; init; } = 0.2;
    public int MaxOutputTokens { get; init; } = 900;
    public bool Disabled { get; init; } = false;
}
