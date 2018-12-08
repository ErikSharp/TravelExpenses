using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence.Extensions;

namespace TravelExpenses.Persistence
{
    /// <summary>
    /// We don't have to create a partial class as this is code first
    /// and the context is created manually
    /// </summary>
    public class TravelExpensesContext : DbContext
    {
        public TravelExpensesContext(DbContextOptions<TravelExpensesContext> options)
            :base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This is a global query filter that will run everytime that we query User
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.Disabled);

            modelBuilder.HasDefaultSchema("app");

            //Great information on mapping the fields
            //https://www.meziantou.net/2017/07/27/entity-framework-core-specifying-data-type-length-and-precision

            modelBuilder.ApplyAllConfigurations();
            base.OnModelCreating(modelBuilder);
        }
    }
}
