using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class FarmStatisticsConfiguration : IEntityTypeConfiguration<FarmStatistics>
{
    public void Configure(EntityTypeBuilder<FarmStatistics> builder)
    {
        builder.ToTable(ConfigurationConstants.FarmStatisticsTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AverageDrinkTime);
        builder.Property(x => x.AverageFeedTime);
        builder.Property(x => x.LastDrinkTime);
        builder.Property(x => x.LastFeedTime);
        builder.Property(x => x.TotalDrinkingsCount);
        builder.Property(x => x.TotalFeedingsCount);
    }
}