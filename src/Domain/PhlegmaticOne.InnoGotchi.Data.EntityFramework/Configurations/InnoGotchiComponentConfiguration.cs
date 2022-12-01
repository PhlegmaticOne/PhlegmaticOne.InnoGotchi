using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class InnoGotchiComponentConfiguration : IEntityTypeConfiguration<InnoGotchiComponent>
{
    public void Configure(EntityTypeBuilder<InnoGotchiComponent> builder)
    {
        builder.ToTable(ConfigurationConstants.InnoGotchiComponentsTableName);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.ImageUrl).IsRequired();

        builder.HasMany(x => x.InnoGotchiModelComponents)
            .WithOne(x => x.InnoGotchiComponent)
            .HasForeignKey(x => x.ComponentId);
    }
}