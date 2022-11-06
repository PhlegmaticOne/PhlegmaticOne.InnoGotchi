using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Data.Models.Enums;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class InnoGotchiConfiguration : IEntityTypeConfiguration<InnoGotchiModel>
{
    public void Configure(EntityTypeBuilder<InnoGotchiModel> builder)
    {
        builder.ToTable(ConfigurationConstants.InnoGothiesTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HungerLevel)
            .HasDefaultValue(HungerLevel.Normal)
            .HasConversion<string>();

        builder.Property(x => x.ThirstyLevel)
            .HasDefaultValue(ThirstyLevel.Normal)
            .HasConversion<string>();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.HasMany(x => x.Components)
            .WithOne(x => x.InnoGotchi)
            .HasForeignKey(x => x.InnoGotchiId);
    }
}