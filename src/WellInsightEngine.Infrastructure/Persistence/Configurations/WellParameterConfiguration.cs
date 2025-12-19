using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class WellParameterConfiguration : IEntityTypeConfiguration<WellParameter>
{
    public void Configure(EntityTypeBuilder<WellParameter> e)
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        e.Property(x => x.DataType)
            .IsRequired();

        e.HasIndex(x => new { x.WellId, x.Name });
    }
}