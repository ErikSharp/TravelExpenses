using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Common
{
    public interface IDateTime
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}
