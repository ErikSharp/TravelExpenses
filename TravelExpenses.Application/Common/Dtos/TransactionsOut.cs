using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class TransactionsOut
    {
        public TransactionOut[] Transactions { get; set; }
        public int TotalRecords { get; set; }
        public int PageCount { get; set; }
    }
}
