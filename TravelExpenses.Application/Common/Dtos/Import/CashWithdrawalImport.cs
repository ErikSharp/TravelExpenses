using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos.Import
{
    public class CashWithdrawalImport
    {
        public string Title { get; set; }
        public string TransDate { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public string Memo { get; set; }
    }
}
