using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Domain.Entities
{
    public class Keyword
    {
        public int Id { get; set; }
        public string KeywordName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<TransactionKeyword> TransactionKeywords { get; private set; }
    }
}
