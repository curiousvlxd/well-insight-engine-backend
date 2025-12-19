using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class WellConfiguration : IEntityTypeConfiguration<Well>
{
    public void Configure(EntityTypeBuilder<Well> e)
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        e.Property(x => x.ExternalId)
            .HasMaxLength(200)
            .IsRequired();

        e.HasIndex(x => x.ExternalId);

        e.HasMany(x => x.Parameters)
            .WithOne(x => x.Well)
            .HasForeignKey(x => x.WellId);

        e.HasMany(x => x.Actions)
            .WithOne(x => x.Well)
            .HasForeignKey(x => x.WellId);
    }
}