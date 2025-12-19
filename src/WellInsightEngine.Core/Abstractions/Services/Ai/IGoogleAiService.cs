using WellInsightEngine.Core.Abstractions.Services.Ai.Contracts;

namespace WellInsightEngine.Core.Abstractions.Services.Ai;

public interface IGoogleAiService
{
    Task<AiResponse> GenerateAsync(string prompt, CancellationToken ct);
}