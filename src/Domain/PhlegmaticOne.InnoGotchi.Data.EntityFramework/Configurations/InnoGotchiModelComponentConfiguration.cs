using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class InnoGotchiModelComponentConfiguration : IEntityTypeConfiguration<InnoGotchiModelComponent>
{
    public void Configure(EntityTypeBuilder<InnoGotchiModelComponent> builder)
    {
        builder.ToTable(ConfigurationConstants.InnoGotchiModelComponentsTableName);

        builder.HasKey(x => new { x.InnoGotchiId, x.ComponentId });

        builder.Property(x => x.ScaleX);

        builder.Property(x => x.ScaleY);

        builder.Property(x => x.TranslationX);

        builder.Property(x => x.TranslationY);

        builder.HasOne(x => x.InnoGotchiComponent)
            .WithMany(x => x.InnoGotchiModelComponents);

        builder.HasOne(x => x.InnoGotchi)
            .WithMany(x => x.Components);
    }
}