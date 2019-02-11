using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class cash_withdrawal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashWithdrawal",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    TransDate = table.Column<DateTime>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashWithdrawal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashWithdrawal_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "app",
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashWithdrawal_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashWithdrawal_CurrencyId",
                schema: "app",
                table: "CashWithdrawal",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CashWithdrawal_UserId",
                schema: "app",
                table: "CashWithdrawal",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashWithdrawal",
                schema: "app");
        }
    }
}
