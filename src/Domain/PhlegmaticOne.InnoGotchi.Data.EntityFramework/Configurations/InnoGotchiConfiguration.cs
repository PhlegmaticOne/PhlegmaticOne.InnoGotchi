using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class InnoGotchiConfiguration : IEntityTypeConfiguration<InnoGotchiModel>
{
    public void Configure(EntityTypeBuilder<InnoGotchiModel> builder)
    {
        builder.ToTable(ConfigurationConstants.InnoGothiesTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HungerLevel)
            .HasDefaultValue(HungerLevel.Normal)
            .HasConversion<int>();

        builder.Property(x => x.ThirstyLevel)
            .HasDefaultValue(ThirstyLevel.Normal)
            .HasConversion<int>();

        builder.Property(x => x.LastDrinkTime);
        builder.Property(x => x.LastFeedTime);
        builder.Property(x => x.AgeUpdatedAt);
        builder.Property(x => x.Age);
        builder.Property(x => x.IsDead).HasDefaultValue(false);
        builder.Property(x => x.HappinessDaysCount);
        builder.Property(x => x.LiveSince);
        builder.Property(x => x.DeadSince);

        builder.HasIndex(x => x.Name);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.HasMany(x => x.Components)
            .WithOne(x => x.InnoGotchi)
            .HasForeignKey(x => x.InnoGotchiId);
    }
}