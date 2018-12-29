using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Persistence.Configurations
{
    public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
    {
        public void Configure(EntityTypeBuilder<Keyword> builder)
        {
            builder.ToTable("Keyword");

            builder.HasIndex(k => new { k.KeywordName, k.UserId }).IsUnique(true);

            builder.Property(e => e.KeywordName)
                    .HasMaxLength(255)
                    .IsRequired();
        }
    }
}
