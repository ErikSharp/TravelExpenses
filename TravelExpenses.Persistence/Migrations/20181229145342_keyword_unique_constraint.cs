using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class keyword_unique_constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Keyword_KeywordName_UserId",
                schema: "app",
                table: "Keyword",
                columns: new[] { "KeywordName", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Keyword_KeywordName_UserId",
                schema: "app",
                table: "Keyword");
        }
    }
}
