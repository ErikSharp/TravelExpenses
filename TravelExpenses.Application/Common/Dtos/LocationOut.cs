using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class LocationOut
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
