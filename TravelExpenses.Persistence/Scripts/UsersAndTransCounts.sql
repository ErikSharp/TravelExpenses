-- users and their transaction counts
select u.Email, u.id as UserId, count(*) as TransactionCount
from app.Users u
left join app.[Transaction] t on u.id = t.UserId
group by u.Email, u.id
order by count(*) desc
