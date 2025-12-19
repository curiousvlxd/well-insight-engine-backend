using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class WellActionConfiguration : IEntityTypeConfiguration<WellAction>
{
    public void Configure(EntityTypeBuilder<WellAction> e)
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.Source).IsRequired();

        e.Property(x => x.Title)
            .HasMaxLength(300)
            .IsRequired();

        e.Property(x => x.Details)
            .HasMaxLength(2000)
            .IsRequired();

        e.HasIndex(x => new { x.WellId, x.Timestamp });
    }
}