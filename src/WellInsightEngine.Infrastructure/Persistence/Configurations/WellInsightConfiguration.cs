using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight;
using WellInsightEngine.Core.Entities.WellInsight.Payload;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class WellInsightConfiguration : IEntityTypeConfiguration<WellInsight>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    public void Configure(EntityTypeBuilder<WellInsight> e)
    {
        e.HasKey(x => x.Id);
        e.Property(x => x.CreatedAt).IsRequired();
        e.Property(x => x.From).IsRequired();
        e.Property(x => x.To).IsRequired();
        e.Property(x => x.Title).IsRequired();
        e.Property(x => x.Summary).IsRequired();
        e.Property(x => x.Slug).IsRequired();
        var payloadConverter = new ValueConverter<WellInsightPayload, string>(
            v => JsonSerializer.Serialize(v, JsonOptions),
            v => JsonSerializer.Deserialize<WellInsightPayload>(v, JsonOptions)!);
        var stringArrayConverter = new ValueConverter<string[], string>(
            v => JsonSerializer.Serialize(v, JsonOptions),
            v => JsonSerializer.Deserialize<string[]>(v, JsonOptions) ?? Array.Empty<string>());
        e.Property(x => x.Payload)
            .HasConversion(payloadConverter)
            .HasColumnType("jsonb")
            .IsRequired();

        e.Property(x => x.Highlights)
            .HasConversion(stringArrayConverter)
            .HasColumnType("jsonb")
            .IsRequired();

        e.Property(x => x.Suspicions)
            .HasConversion(stringArrayConverter)
            .HasColumnType("jsonb")
            .IsRequired();

        e.Property(x => x.RecommendedActions)
            .HasConversion(stringArrayConverter)
            .HasColumnType("jsonb")
            .IsRequired();

        e.HasOne<Well>()
            .WithMany()
            .HasForeignKey(x => x.WellId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasIndex(x => new { x.WellId, x.CreatedAt });
        e.HasIndex(x => new { x.WellId, x.From, x.To });
    }
}
