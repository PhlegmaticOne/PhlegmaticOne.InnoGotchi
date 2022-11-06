using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable(ConfigurationConstants.UserProfilesTableName);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.SecondName).IsRequired();
        builder.Property(x => x.AvatarData);
        builder.Property(x => x.JoinDate);

        builder.HasOne(x => x.User)
            .WithOne(x => x.Profile)
            .HasForeignKey<User>(x => x.ProfileId);

        builder.HasOne(x => x.Farm)
            .WithOne(x => x.Owner)
            .HasForeignKey<Farm>(x => x.OwnerId);

        builder.HasMany(x => x.Collaborations)
            .WithOne(x => x.Collaborator)
            .HasForeignKey(x => x.UserProfileId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}