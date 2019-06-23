using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class TransactionCreateIn
    {
        public string TransDate { get; set; }
        public decimal Amount { get; set; }
        public int? LocationId { get; set; }
        public int CurrencyId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
        public bool PaidWithCash { get; set; }
        public int UserId { get; set; }
        public int[] KeywordIds { get; set; }
    }
}
