using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class InnoGotchiModelComponentConfiguration : IEntityTypeConfiguration<InnoGotchiModelComponent>
{
    public void Configure(EntityTypeBuilder<InnoGotchiModelComponent> builder)
    {
        builder.ToTable(ConfigurationConstants.InnoGotchiModelComponentsTableName);
        builder.HasKey(x => new { x.InnoGotchiId, x.ComponentId });
    }
}