using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CurrencyOut
    {
        public int Id { get; set; }
        public string IsoCode { get; set; }
        public string CurrencyName { get; set; }
    }
}
