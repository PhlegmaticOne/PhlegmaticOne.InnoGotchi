using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class CollaborationConfiguration : IEntityTypeConfiguration<Collaboration>
{
    public void Configure(EntityTypeBuilder<Collaboration> builder)
    {
        builder.ToTable(ConfigurationConstants.CollaborationsTableName);
        builder.HasKey(x => new { x.UserProfileId, x.FarmId });
    }
}