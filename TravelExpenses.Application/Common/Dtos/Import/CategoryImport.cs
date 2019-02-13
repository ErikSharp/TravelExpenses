using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos.Import
{
    public class CategoryImport
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string IncomeExpense { get; set; }
    }
}
