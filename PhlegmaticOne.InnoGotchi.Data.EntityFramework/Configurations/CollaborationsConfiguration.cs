using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class CollaborationsConfiguration : IEntityTypeConfiguration<Collaboration>
{
    public void Configure(EntityTypeBuilder<Collaboration> builder)
    {
        builder.ToTable(ConfigurationConstants.CollaborationsTableName);

        builder.HasKey(c => c.Id);

        builder.HasOne(x => x.Collaborator)
            .WithMany(x => x.Collaborations)
            .HasForeignKey(x => x.UserProfileId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Farm)
            .WithMany(x => x.Collaborations)
            .HasForeignKey(x => x.FarmId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}