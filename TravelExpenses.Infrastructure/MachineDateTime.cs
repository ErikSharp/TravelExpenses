using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Common;

namespace TravelExpenses.Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
