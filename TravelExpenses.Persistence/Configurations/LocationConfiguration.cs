﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Persistence.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            builder.HasIndex(c => new { c.LocationName, c.CountryId, c.UserId }).IsUnique(true);

            builder.Property(e => e.LocationName)
                    .HasMaxLength(255)
                    .IsRequired();

            builder
                .HasOne(l => l.User)
                .WithMany(u => u.Locations)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
