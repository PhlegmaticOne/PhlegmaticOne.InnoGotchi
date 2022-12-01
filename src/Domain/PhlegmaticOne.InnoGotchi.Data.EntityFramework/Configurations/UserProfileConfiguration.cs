using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable(ConfigurationConstants.UserProfilesTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).IsRequired();

        builder.Property(x => x.LastName).IsRequired();

        builder.Property(x => x.JoinDate);

        builder.HasOne(x => x.Avatar)
            .WithOne(x => x.UserProfile)
            .HasForeignKey<UserProfile>(x => x.AvatarId);

        builder.HasOne(x => x.Farm)
            .WithOne(x => x.Owner)
            .HasForeignKey<Farm>(x => x.OwnerId);
    }
}