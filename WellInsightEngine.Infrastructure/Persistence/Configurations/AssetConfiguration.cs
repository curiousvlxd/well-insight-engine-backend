using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> e)
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        e.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasMany(x => x.Wells)
            .WithOne(x => x.Asset)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasIndex(x => x.ParentId);
        e.HasIndex(x => new { x.ParentId, x.Name });
    }
}