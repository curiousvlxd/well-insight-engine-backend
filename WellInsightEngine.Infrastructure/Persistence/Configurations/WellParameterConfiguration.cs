using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class WellParameterConfiguration : IEntityTypeConfiguration<WellParameter>
{
    public void Configure(EntityTypeBuilder<WellParameter> e)
    {
        e.HasKey(x => new { x.WellId, x.ParameterId });

        e.HasOne(x => x.Well)
            .WithMany(x => x.Parameters)
            .HasForeignKey(x => x.WellId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Parameter)
            .WithMany(x => x.Wells)
            .HasForeignKey(x => x.ParameterId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasIndex(x => x.ParameterId);
    }
}