using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CurrencyTotalsRequest
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int CurrencyId { get; set; }
    }
}
