using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class transaction_keyword_trigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var trigger =
@"CREATE OR ALTER TRIGGER [app].[transactionkeyword_insert] ON [app].[TransactionKeyword]
AFTER INSERT, UPDATE
AS

DECLARE @KeywordId INT, @TransactionId INT, @TransactionUserId INT, @KeywordUserId INT

select @KeywordId = KeywordId, @TransactionId = TransactionId from inserted

select @TransactionUserId = UserId from [app].[Transaction] where Id = @TransactionId

select @KeywordUserId = UserId from [app].[Keyword] where Id = @KeywordId

if @TransactionUserId <> @KeywordUserId
begin
	rollback transaction;
	THROW 51000, 'The keyword does not exist.', 1;
	RETURN
end;";

            migrationBuilder.Sql(trigger);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql =
@"DROP TRIGGER [app].[transactionkeyword_insert]
GO";
            migrationBuilder.Sql(sql);
        }
    }
}
