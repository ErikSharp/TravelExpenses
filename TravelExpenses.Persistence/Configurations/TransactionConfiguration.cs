using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(e => e.TransDate)
                    .HasColumnType("date")
                    .IsRequired();

            builder.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("smallmoney");

            //builder.Property(e => e.Location).IsRequired();
            //builder.Property(e => e.Currency).IsRequired();
            //builder.Property(e => e.Category).IsRequired();

            builder.Property(e => e.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.Memo)
                .IsRequired(false);

            builder.Property(e => e.PaidWithCash)
                .IsRequired();

            builder
                .HasOne(t => t.Currency)
                .WithMany(c => c.Transactions)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.Location)
                .WithMany(l => l.Transactions)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
