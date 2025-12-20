using System.Text.Json;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using WellInsightEngine.Core.Abstractions;
using WellInsightEngine.Core.Abstractions.Services.Ai;
using WellInsightEngine.Core.Abstractions.Services.Ai.Contracts;
using WellInsightEngine.Infrastructure.Services.Ai.Options;

namespace WellInsightEngine.Infrastructure.Services.Ai;

public sealed class GoogleAiService(Client client, IOptions<AiOptions> options) : IGoogleAiService
{   
    private readonly AiOptions _options = options.Value;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public async Task<AiResponse> GenerateAsync(string prompt, CancellationToken ct)
    {
        var config = new GenerateContentConfig
        {
            Temperature = _options.Temperature,
            MaxOutputTokens = _options.MaxOutputTokens
        };
        var response = await client.Models.GenerateContentAsync(model: _options.Model, contents: prompt, config: config);
        var text = response.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(text))
            return new AiResponse();

        try
        {
            return JsonSerializer.Deserialize<AiResponse>(text, JsonOptions) ?? new AiResponse();
        }
        catch
        {
            return new AiResponse();
        }
    }
}

public sealed class DisabledGoogleAiService : IGoogleAiService
{
    public Task<AiResponse> GenerateAsync(string prompt, CancellationToken cancellationToken)
        => Task.FromResult(new AiResponse());
}