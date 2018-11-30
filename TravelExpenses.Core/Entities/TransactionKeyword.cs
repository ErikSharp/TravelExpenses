using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class TransactionKeyword
    {
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
