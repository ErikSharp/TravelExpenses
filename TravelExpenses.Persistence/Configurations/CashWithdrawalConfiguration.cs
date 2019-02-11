using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Persistence.Configurations
{
    public class CashWithdrawalConfiguration : IEntityTypeConfiguration<CashWithdrawal>
    {
        public void Configure(EntityTypeBuilder<CashWithdrawal> builder)
        {
            builder.ToTable("CashWithdrawal");

            builder.Property(e => e.TransDate)
                    .HasColumnType("date")
                    .IsRequired();

            builder.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(e => e.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.Memo)
                .IsRequired(false);

            builder
                .HasOne(t => t.Currency)
                .WithMany(c => c.CashWithdrawals)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.User)
                .WithMany(u => u.CashWithdrawals)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
