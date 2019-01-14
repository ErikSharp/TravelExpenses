using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class BaseRequirements
    {
        public bool HasLocation { get; set; }
        public bool HasCategory { get; set; }
        public bool HasKeyword { get; set; }
    }
}
