using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.IntegrationTests
{
    public static class SeedData
    {
        public static void PopulateTestData(TravelExpensesContext dbContext)
        {
            dbContext.Users.Add(new User
            {
                Id = 1,
                Email = @"erik.sharp@hadleyshope.com",
                PasswordHash = @"$2y$12$yVYkJsR7a4Wj3wRzCD9Pn.DvDGWY3Dzx2AwisSqailn3Pyu.X.zWi",
                Created = DateTime.Now,
                Disabled = false
            });

            dbContext.SaveChanges();
        }
    }
}
