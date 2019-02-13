using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos.Import
{
    public class LocationImport
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int CountryId { get; set; }
        public int DefaultCurrencyId { get; set; }
    }
}
