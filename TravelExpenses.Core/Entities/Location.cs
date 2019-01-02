using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Transaction> Transactions { get; private set; }
    }
}
