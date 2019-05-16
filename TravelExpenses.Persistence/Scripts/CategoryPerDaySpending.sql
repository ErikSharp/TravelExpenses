select * from app.users

select * from app.Location where UserId = 23

select * from app.[Transaction] where LocationId = 106

select min(transdate), max(transdate) from app.[Transaction] where LocationId = 106

select datediff(day, min(transdate), max(transdate)) + 1 as days from app.[Transaction] where LocationId = 106

select * from app.Currency where IsoCode = 'THB'

select 
	c.CategoryName, 
	count(*) [Transaction Count],
	sum(t.Amount) as [Total THB], 
	(sum(t.Amount) / (select datediff(day, min(transdate), max(transdate)) + 1 from app.[Transaction] where LocationId = 106)) as [Per Day THB]
from app.[Transaction] t
join app.Category c on t.CategoryId = c.Id
where LocationId = 106
and CurrencyId = 139
and c.CategoryName not in ('Non-trip', 'Medical', 'Loss/Gain', 'Fees')
group by c.CategoryName
order by sum(t.Amount) desc