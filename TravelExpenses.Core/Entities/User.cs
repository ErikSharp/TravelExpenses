using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime Created { get; set; }
        public bool Disabled { get; set; }

        public ICollection<Location> Locations { get; private set; }
        public ICollection<Country> Countries { get; private set; }
        public ICollection<Category> Categories { get; private set; }
        public ICollection<Currency> Currencies { get; private set; }
        public ICollection<Keyword> Keywords { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; }
    }
}
