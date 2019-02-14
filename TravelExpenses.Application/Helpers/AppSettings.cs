using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelExpenses.Application.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int RecentTransactionsTakeAmount { get; set; }
        public string AdminUserIdsCsv { get; set; }

        public int[] AdminUserIds
        {
            get
            {
                return AdminUserIdsCsv
                    .Split(',')
                    .Select(id => int.Parse(id))
                    .ToArray();
            }
        }
    }
}

