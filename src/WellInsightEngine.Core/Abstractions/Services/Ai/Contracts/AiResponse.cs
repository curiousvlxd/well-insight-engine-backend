using System.Text.Json.Serialization;

namespace WellInsightEngine.Core.Abstractions.Services.Ai.Contracts;

public sealed record AiResponse
{   
    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;
    [JsonPropertyName("summary")]
    public string Summary { get; init; } = string.Empty;
    [JsonPropertyName("payloadJson")]
    public string PayloadJson { get; init; } = string.Empty;
}