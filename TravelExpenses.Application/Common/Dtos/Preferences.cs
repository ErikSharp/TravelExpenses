using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class Preferences
    {
        public bool ShowReconcileInstructions { get; set; }
        public int DefaultQueryCurrencyId { get; set; }
    }
}
