using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class inserttransactionsp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = 
@"USE TravelExpenses

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [app].[usp_transaction_insert]

	@TransDate date,
	@Amount smallmoney,
	@LocationId int,
	@CurrencyId int,
	@CategoryId int,
	@Title nvarchar(255),
	@Memo nvarchar(max),
	@PaidWithCash bit,
	@UserId int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @count as INT

	-- ensure the location exists and belongs to the user
	select @count = count(*) from [app].[Location] where Id = @LocationId and UserId = @UserId

	IF @count = 0
		BEGIN;
		THROW 51000, 'The location does not exist.', 1;
		END

	-- ensure the currency exists and belongs to the user
	select @count = count(*) from [app].[Currency] where Id = @CurrencyId and UserId = @UserId

	IF @count = 0
		BEGIN;
		THROW 51000, 'The currency does not exist.', 1;
		END

	-- ensure the category exists and belongs to the user
	select @count = count(*) from [app].[Category] where Id = @CategoryId and UserId = @UserId

	IF @count = 0
		BEGIN;
		THROW 51000, 'The category does not exist.', 1;
		END

	INSERT INTO [app].[Transaction]
           ([TransDate]
           ,[Amount]
           ,[LocationId]
           ,[CurrencyId]
           ,[CategoryId]
           ,[Title]
           ,[Memo]
           ,[PaidWithCash]
           ,[UserId])
     VALUES
           (@TransDate
           ,@Amount
           ,@LocationId
           ,@CurrencyId
           ,@CategoryId
           ,@Title
           ,@Memo
           ,@PaidWithCash
           ,@UserId)

END
GO";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"DROP PROCEDURE [app].[usp_transaction_insert];";
            migrationBuilder.Sql(sql);
        }
    }
}
