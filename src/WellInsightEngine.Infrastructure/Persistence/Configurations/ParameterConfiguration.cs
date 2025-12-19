using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class ParameterConfiguration : IEntityTypeConfiguration<Parameter>
{
    public void Configure(EntityTypeBuilder<Parameter> e)
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.Code).HasMaxLength(100).IsRequired();
        e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        e.Property(x => x.DataType).HasConversion<short>().IsRequired();

        e.HasIndex(x => x.Code).IsUnique();
    }
}