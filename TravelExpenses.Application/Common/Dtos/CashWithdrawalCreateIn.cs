using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CashWithdrawalCreateIn
    {
        public string Title { get; set; }
        public string TransDate { get; set; }
        public decimal Amount { get; set; }        
        public int CurrencyId { get; set; }
        public int LocationId { get; set; }
        public string Memo { get; set; }
        public int UserId { get; set; }
    }
}
