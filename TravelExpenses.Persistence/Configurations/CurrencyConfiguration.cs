using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Persistence.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currency");

            builder.Property(e => e.IsoCode)
                    .HasMaxLength(3)
                    .IsRequired();

            builder.Property(e => e.CurrencyName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.IsHomeCurrency)
                .IsRequired();

            builder.Property(e => e.HomeCurrencyRatio)
                .IsRequired();
        }
    }
}
