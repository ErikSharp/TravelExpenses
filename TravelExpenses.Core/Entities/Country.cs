using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }

        public ICollection<Location> Locations { get; private set; }
    }
}
