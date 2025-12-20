using System.Text.Json.Serialization;

namespace WellInsightEngine.Core.Abstractions.Services.Ai.Contracts;

public sealed record AiResponse
{   
    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;
    [JsonPropertyName("summary")]
    public string Summary { get; init; } = string.Empty;
    [JsonPropertyName("highlights")]
    public string[] Highlights { get; init; } = [];
    [JsonPropertyName("suspicions")]
    public string[] Suspicions { get; init; } = [];
    [JsonPropertyName("recommendedActions")]
    public string[] RecommendedActions { get; init; } = [];
}