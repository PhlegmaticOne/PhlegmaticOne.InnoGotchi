﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Configurations;

public class FarmConfiguration : IEntityTypeConfiguration<Farm>
{
    public void Configure(EntityTypeBuilder<Farm> builder)
    {
        builder.ToTable(ConfigurationConstants.FarmsTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasMany(x => x.InnoGotchies)
            .WithOne(x => x.Farm);

        builder.HasMany(x => x.Collaborations)
            .WithOne(x => x.Farm)
            .HasForeignKey(x => x.FarmId);
    }
}