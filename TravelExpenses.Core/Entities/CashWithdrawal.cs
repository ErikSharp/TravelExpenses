using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class CashWithdrawal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime TransDate { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Memo { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
