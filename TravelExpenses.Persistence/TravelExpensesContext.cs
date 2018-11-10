using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Persistence
{
    public class TravelExpensesContext : DbContext
    {
        public TravelExpensesContext(DbContextOptions<TravelExpensesContext> options)
            :base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");

            modelBuilder.Entity<User>(entity =>
            {
                //https://7php.com/the-maximum-length-limit-of-an-email-address-is-254-not-320/
                //https://stackoverflow.com/questions/3844431/are-email-addresses-allowed-to-contain-non-alphanumeric-characters
                entity.Property(e => e.Email)
                    .HasMaxLength(254)
                    .IsUnicode()
                    .IsRequired();

                entity.HasIndex(e => e.Email).IsUnique().HasName("Index_Email");

                //https://stackoverflow.com/questions/5881169/what-column-type-length-should-i-use-for-storing-a-bcrypt-hashed-password-in-a-d
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Created)
                    .HasDefaultValueSql("getdate()")
                    .ValueGeneratedOnAdd();
            });
        }
    }
}
