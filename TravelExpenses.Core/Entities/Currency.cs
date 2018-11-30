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
        public bool IsHomeCurrency { get; set; }
        public double HomeCurrencyRatio { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Location> Locations { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; }
    }
}
