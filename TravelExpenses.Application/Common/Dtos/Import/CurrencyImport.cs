using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos.Import
{
    public class CurrencyImport
    {
        public int CurrencyId { get; set; }
        public string IsoCode { get; set; }
        public string CurrencyName { get; set; }
        public bool HomeCurrency { get; set; }
        public decimal HomeCurrencyRatio { get; set; }
    }
}
