using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int RecentTransactionsTakeAmount { get; set; }
        public int ErikUserId { get; set; }
    }
}

