﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelExpenses.Persistence;

namespace TravelExpenses.Persistence.Migrations
{
    [DbContext(typeof(TravelExpensesContext))]
    partial class TravelExpensesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("app")
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TravelExpenses.Domain.Entities.CashWithdrawal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<int>("CurrencyId");

                    b.Property<int?>("LocationId");

                    b.Property<string>("Memo");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("TransDate")
                        .HasColumnType("date");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("CashWithdrawal");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("Color")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(6591981);

                    b.Property<string>("Icon")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(40)
                        .HasDefaultValue("live_help");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("CategoryName", "UserId")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("CountryName")
                        .IsUnique();

                    b.ToTable("Country");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CurrencyName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("IsoCode")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.HasKey("Id");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("KeywordName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("KeywordName", "UserId")
                        .IsUnique();

                    b.ToTable("Keyword");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryId");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("UserId");

                    b.HasIndex("LocationName", "CountryId", "UserId")
                        .IsUnique();

                    b.ToTable("Location");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("smallmoney");

                    b.Property<int>("CategoryId");

                    b.Property<int>("CurrencyId");

                    b.Property<int>("LocationId");

                    b.Property<string>("Memo");

                    b.Property<bool>("PaidWithCash");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("TransDate")
                        .HasColumnType("date");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.TransactionKeyword", b =>
                {
                    b.Property<int>("TransactionId");

                    b.Property<int>("KeywordId");

                    b.HasKey("TransactionId", "KeywordId");

                    b.HasIndex("KeywordId");

                    b.ToTable("TransactionKeyword");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("Disabled");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .IsUnicode(true);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false);

                    b.Property<string>("Preferences");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("Index_Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.CashWithdrawal", b =>
                {
                    b.HasOne("TravelExpenses.Domain.Entities.Currency", "Currency")
                        .WithMany("CashWithdrawals")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TravelExpenses.Domain.Entities.Location", "Location")
                        .WithMany("CashWithdrawals")
                        .HasForeignKey("LocationId");

                    b.HasOne("TravelExpenses.Domain.Entities.User", "User")
                        .WithMany("CashWithdrawals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Category", b =>
                {
                    b.HasOne("TravelExpenses.Domain.Entities.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Keyword", b =>
                {
                    b.HasOne("TravelExpenses.Domain.Entities.User", "User")
                        .WithMany("Keywords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Location", b =>
                {
                    b.HasOne("TravelExpenses.Domain.Entities.Country", "Country")
                        .WithMany("Locations")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelExpenses.Domain.Entities.User", "User")
                        .WithMany("Locations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("TravelExpenses.Domain.Entities.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelExpenses.Domain.Entities.Currency", "Currency")
                        .WithMany("Transactions")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TravelExpenses.Domain.Entities.Location", "Location")
                        .WithMany("Transactions")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TravelExpenses.Domain.Entities.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TravelExpenses.Domain.Entities.TransactionKeyword", b =>
                {
                    b.HasOne("TravelExpenses.Domain.Entities.Keyword", "Keyword")
                        .WithMany("TransactionKeywords")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TravelExpenses.Domain.Entities.Transaction", "Transaction")
                        .WithMany("TransactionKeywords")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
