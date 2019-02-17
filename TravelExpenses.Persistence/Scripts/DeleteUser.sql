-- Delete user
DECLARE @UserId INT = 1025

delete from app.[TransactionKeyword]
where TransactionId in (
	select t.Id from app.[Transaction] t where t.UserId = @UserId
)

delete from app.[Transaction]
where UserId = @UserId

delete from app.[CashWithdrawal]
where UserId = @UserId

delete from app.Category
where UserId = @UserId

delete from app.[Location]
where UserId = @UserId

delete from app.[Keyword]
where UserId = @UserId

delete from app.[Users]
where Id = @UserId
