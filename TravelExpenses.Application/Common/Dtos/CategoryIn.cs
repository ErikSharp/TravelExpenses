using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CategoryIn : CategoryBase
    {
        public int UserId { get; set; }
    }
}
