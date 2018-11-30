using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Transaction> Transactions { get; private set; }
    }
}
