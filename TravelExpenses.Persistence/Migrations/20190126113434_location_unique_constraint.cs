using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class location_unique_constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Location_LocationName_CountryId_UserId",
                schema: "app",
                table: "Location",
                columns: new[] { "LocationName", "CountryId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Location_LocationName_CountryId_UserId",
                schema: "app",
                table: "Location");
        }
    }
}
