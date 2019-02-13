using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos.Import
{
    public class UserImportDto
    {
        public CountryImport[] Countries { get; set; }
        public CurrencyImport[] Currencies { get; set; }
        public KeywordImport[] Keywords { get; set; }
        public CategoryImport[] Categories { get; set; }
        public LocationImport[] Locations { get; set; }
        public TransactionImport[] Transactions { get; set; }
        public TransactionKeywordsImport[] TransactionKeywords { get; set; }
        public CashWithdrawalImport[] CashWithdrawals { get; set; }
    }
}
