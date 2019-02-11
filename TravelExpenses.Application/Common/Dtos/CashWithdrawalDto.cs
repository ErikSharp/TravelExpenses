using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CashWithdrawalDto : CashWithdrawalCreateIn
    {
        public int Id { get; set; }
    }
}
