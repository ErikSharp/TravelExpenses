select 
	l.LocationName, 
	c.CurrencyName, 
	SUM(t.Amount) as Total, 
	MIN(t.TransDate) as MinTransDate,
	MAX(t.TransDate) as MaxTransDate,
	DATEDIFF(DAY, MIN(t.TransDate), MAX(t.TransDate)) + 1 as DaysDuration,
	SUM(t.Amount) / (DATEDIFF(DAY, MIN(t.TransDate), MAX(t.TransDate)) + 1) as PerDay
from app.[Transaction] t
inner join app.[Location] l on t.LocationId = l.Id
inner join app.[Currency] c on t.CurrencyId = c.Id
where t.UserId = 1029 and t.CategoryId in (1351, 1348, 1353, 1354, 1347, 1346, 1352)
group by l.LocationName, c.CurrencyName
order by l.LocationName, DaysDuration desc