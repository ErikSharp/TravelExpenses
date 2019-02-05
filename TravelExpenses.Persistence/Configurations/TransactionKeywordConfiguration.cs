using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Persistence.Configurations
{
    public class TransactionKeywordConfiguration : IEntityTypeConfiguration<TransactionKeyword>
    {
        public void Configure(EntityTypeBuilder<TransactionKeyword> builder)
        {
            builder.ToTable("TransactionKeyword");

            builder.HasKey(tk => new { tk.TransactionId, tk.KeywordId });

            builder
                .HasOne(tk => tk.Transaction)
                .WithMany(t => t.TransactionKeywords)
                .HasForeignKey(tk => tk.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(tk => tk.Keyword)
                .WithMany(k => k.TransactionKeywords)
                .HasForeignKey(tk => tk.KeywordId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
