using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class transaction_insert_trigger_change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var trigger =
@"CREATE OR ALTER TRIGGER [app].[transaction_insert] ON [app].[Transaction]
AFTER INSERT, UPDATE
AS

DECLARE @LocationId INT, @CategoryId INT, @CurrencyId INT, @UserId INT

select @LocationId = LocationId, @CategoryId = CategoryId, @CurrencyId = CurrencyId, @UserId = UserId from inserted

if not exists (select * from [app].[Location] where Id = @LocationId and UserId = @UserId)
begin
	rollback transaction;
	THROW 51000, 'The location does not exist.', 1;
	RETURN
end;

if not exists (select * from [app].Category where Id = @CategoryId and UserId = @UserId)
begin
	rollback transaction;
	THROW 51000, 'The category does not exist.', 1;
	RETURN
end;";

            migrationBuilder.Sql(trigger);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var trigger =
@"CREATE OR ALTER TRIGGER [app].[transaction_insert] ON [app].[Transaction]
AFTER INSERT, UPDATE
AS

DECLARE @LocationId INT, @CategoryId INT, @CurrencyId INT, @UserId INT

select @LocationId = LocationId, @CategoryId = CategoryId, @CurrencyId = CurrencyId, @UserId = UserId from inserted

if not exists (select * from [app].[Location] where Id = @LocationId and UserId = @UserId)
begin
	rollback transaction;
	THROW 51000, 'The location does not exist.', 1;
	RETURN
end;

if not exists (select * from [app].Category where Id = @CategoryId and UserId = @UserId)
begin
	rollback transaction;
	THROW 51000, 'The category does not exist.', 1;
	RETURN
end;

if not exists (select * from [app].[Currency] where Id = @CurrencyId and UserId = @UserId)
begin
	rollback transaction;
	THROW 51000, 'The currency does not exist.', 1;
	RETURN
end;";

            migrationBuilder.Sql(trigger);
        }
    }
}
