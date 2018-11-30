using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime TransDate { get; set; }
        public decimal Amount { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
        public bool PaidWithCash { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<TransactionKeyword> TransactionKeywords { get; private set; }
    }
}
