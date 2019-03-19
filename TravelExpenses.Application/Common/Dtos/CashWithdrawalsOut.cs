using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CashWithdrawalsOut
    {
        public CashWithdrawalDto[] CashWithdrawals { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
    }
}
