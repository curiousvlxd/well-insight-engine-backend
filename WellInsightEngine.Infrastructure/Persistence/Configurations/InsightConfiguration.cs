using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class InsightConfiguration : IEntityTypeConfiguration<Insight>
{
    public void Configure(EntityTypeBuilder<Insight> e)
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.CreatedAt).IsRequired();
        e.Property(x => x.From).IsRequired();
        e.Property(x => x.To).IsRequired();

        e.Property(x => x.Status).HasConversion<short>().IsRequired();

        e.Property(x => x.Title).HasMaxLength(300).IsRequired();
        e.Property(x => x.Summary).HasMaxLength(8000).IsRequired();
        e.Property(x => x.Payload).HasColumnType("jsonb").IsRequired();

        e.HasOne<Well>()
            .WithMany()
            .HasForeignKey(x => x.WellId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasIndex(x => new { x.WellId, x.CreatedAt });
        e.HasIndex(x => new { x.WellId, x.From, x.To });
    }
}