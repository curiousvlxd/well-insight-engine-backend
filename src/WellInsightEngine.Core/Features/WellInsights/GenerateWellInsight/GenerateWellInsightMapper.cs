using Riok.Mapperly.Abstractions;
using WellInsightEngine.Core.Abstractions.Services.Ai.Contracts;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;

[Mapper]
public static partial class GenerateWellInsightMapper
{
    public static partial WellInsightAiEnvelope ToEnvelope(AiResponse src);
}