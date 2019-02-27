using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class CategoryOut
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int Color { get; set; }
        public string Icon { get; set; }
    }
}
