-- View all logs
SELECT TOP (1000) [Id]
      ,[Message]
      ,[Level]
      ,[TimeStamp]
      ,[Exception]
  FROM [TravelExpenses].[dbo].[Logs]
  order by id desc

-- Warning and above
SELECT TOP (1000) [Id]
      ,[Message]
      ,[Level]
      ,[TimeStamp]
      ,[Exception]
  FROM [TravelExpenses].[dbo].[Logs]
  where level > 2
  order by id desc

-- Warning and above counts per day
select convert(varchar, l.[TimeStamp], 23) as day, count(*) as WarningErrorCount
from [TravelExpenses].[dbo].[Logs] l
where level > 2
group by convert(varchar, l.[TimeStamp], 23)