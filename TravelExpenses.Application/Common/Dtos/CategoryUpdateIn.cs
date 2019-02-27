using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CategoryUpdateIn : CategoryIn
    {
        public int Id { get; set; }
    }
}
