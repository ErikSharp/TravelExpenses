using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class smallmoney_to_money : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql =
@"ALTER TABLE [app].[Transaction]
ALTER COLUMN [Amount] money;";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql =
@"ALTER TABLE [app].[Transaction]
ALTER COLUMN [Amount] smallmoney;";

            migrationBuilder.Sql(sql);
        }
    }
}
