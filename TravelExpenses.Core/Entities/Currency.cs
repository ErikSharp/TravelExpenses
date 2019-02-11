using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public string IsoCode { get; set; }
        public string CurrencyName { get; set; }
        
        public ICollection<Transaction> Transactions { get; private set; }
        public ICollection<CashWithdrawal> CashWithdrawals { get; private set; }
    }
}
