using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable(ConfigurationConstants.InvitationsTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SentAt);

        builder.Property(x => x.InvitationStatus)
            .HasConversion<string>();

        builder.HasOne(x => x.From)
            .WithMany(x => x.SentInvitations)
            .HasForeignKey(x => x.FromProfileId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.To)
            .WithMany(x => x.ReceivedInvitations)
            .HasForeignKey(x => x.ToProfileId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.To);
    }
}