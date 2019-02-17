-- View user data
DECLARE @UserId INT = 1028

select * from app.[Users] where id = @UserId

SELECT TOP (1000) [Id]
      ,[CategoryName]
      ,[UserId]
  FROM [TravelExpenses].[app].[Category]
  where UserId = @UserId

select * from app.Keyword where userid = @UserId

select * from app.Location where userid = @UserId

select * from app.[Transaction] where userid = @UserId

select * from app.[CashWithdrawal] where userid = @UserId

