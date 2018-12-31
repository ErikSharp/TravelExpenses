using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasIndex(c => new { c.CategoryName, c.UserId }).IsUnique(true);

            builder.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .IsRequired();
        }
    }
}
