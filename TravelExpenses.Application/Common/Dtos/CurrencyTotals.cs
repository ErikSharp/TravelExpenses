using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CurrencyTotals
    {
        public decimal TotalSpent { get; set; }
        public decimal TotalWithdrawn { get; set; }
        public decimal TotalLossGain { get; set; }
    }
}
