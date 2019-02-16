using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Common.Dtos.Import;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Application.Features.CashWithdrawals;
using TravelExpenses.Application.Features.Categories;
using TravelExpenses.Application.Features.Countries;
using TravelExpenses.Application.Features.Currencies;
using TravelExpenses.Application.Features.Keywords;
using TravelExpenses.Application.Features.Locations;
using TravelExpenses.Application.Features.Transactions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Utilities
{
    public class ImportUser
    {
        public class Command : IRequest
        {
            public Command(UserImportDto import, int userId)
            {
                Import = import;
                UserId = userId;
            }

            public UserImportDto Import { get; }
            public int UserId { get; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TravelExpensesContext context;
            private readonly IMediator mediator;

            public Handler(
                TravelExpensesContext context,
                IMediator mediator)
            {
                this.context = context;
                this.mediator = mediator;
            }

            protected async override Task Handle(Command request, CancellationToken response)
            {
                var import = request.Import;

                //ensure that the user exists and that it's empty
                await DeleteUserData(request.UserId);

                //get currency map
                var dbCurrencies = await mediator.Send(new GetCurrencies.Query()).ConfigureAwait(false);

                var ccyMap = new Dictionary<int, int>();
                foreach (var ccy in import.Currencies)
                {
                    var dbMatch = dbCurrencies.First(d => d.IsoCode == ccy.IsoCode);
                    ccyMap.Add(ccy.CurrencyId, dbMatch.Id);
                }

                //get country map
                var dbCountries = await mediator.Send(new GetCountries.Query()).ConfigureAwait(false);

                var countryMap = new Dictionary<int, int>();
                foreach (var country in import.Countries)
                {
                    var dbMatch = dbCountries.First(d => d.CountryName == country.CountryName);
                    countryMap.Add(country.CountryId, dbMatch.Id);
                }

                //enter keywords
                var keywords = import.Keywords.Select(k => new Keyword
                {
                    KeywordName = k.Keyword,
                    UserId = request.UserId
                }).ToArray();

                var newKeywords = await mediator.Send(new CreateKeyword.Query(keywords)).ConfigureAwait(false);

                var keywordMap = new Dictionary<int, int>();
                foreach (var kw in import.Keywords)
                {
                    var dbMatch = newKeywords.First(nk => nk.KeywordName == kw.Keyword);
                    keywordMap.Add(kw.Id, dbMatch.Id);
                }

                //enter categories
                var categories = import.Categories.Select(c => new Category
                {
                    CategoryName = c.CategoryName,
                    UserId = request.UserId
                }).ToArray();

                var newCategories = await mediator.Send(new CreateCategory.Query(categories)).ConfigureAwait(false);

                var categoryMap = new Dictionary<int, int>();
                foreach (var cat in import.Categories)
                {
                    var dbMatch = newCategories.First(nc => nc.CategoryName == cat.CategoryName);
                    categoryMap.Add(cat.CategoryId, dbMatch.Id);
                }

                //enter locations
                LocationOut[] newLocations = null;
                foreach (var location in import.Locations)
                {
                    var newLocation = new Location
                    {
                        LocationName = location.LocationName,
                        CountryId = countryMap[location.CountryId],
                        UserId = request.UserId
                    };

                    newLocations = await mediator.Send(new CreateLocation.Query(newLocation)).ConfigureAwait(false);
                }

                var locationMap = new Dictionary<int, int>();

                if (newLocations.Any())
                {
                    foreach (var loc in import.Locations)
                    {
                        var dbMatch = newLocations.First(nl => nl.LocationName == loc.LocationName);
                        locationMap.Add(loc.LocationId, dbMatch.Id);
                    }
                }

                //enter transactions
                int transCounter = 0;
                foreach (var t in import.Transactions)
                {
                    transCounter++;

                    if (transCounter % 50 == 0)
                    {
                        Log.Information($"ImportUser: on transaction {transCounter}");
                    }

                    var foo = import.TransactionKeywords.Where(tk => tk.TransactionId == t.Id);
                    var newTransKeywords = foo.Select(f => keywordMap[f.KeywordId]).ToArray();

                    var newTrans = new TransactionCreateIn
                    {
                        TransDate = t.TransDate.Substring(0, 10),
                        Amount = t.Amount,
                        LocationId = locationMap[t.LocationId],
                        CurrencyId = ccyMap[t.CurrencyId],
                        CategoryId = categoryMap[t.CategoryId],
                        Title = t.Title,
                        Memo = t.Memo,
                        PaidWithCash = t.PaidWithCash,
                        UserId = request.UserId,
                        KeywordIds = newTransKeywords
                    };

                    await mediator.Send(new CreateTransaction.Command(newTrans, request.UserId)).ConfigureAwait(false);
                }

                //enter cash withdrawals
                foreach (var cw in import.CashWithdrawals)
                {
                    var newCashWithdrawal = new CashWithdrawalCreateIn
                    {
                        Title = cw.Title,
                        TransDate = cw.TransDate,
                        Amount = cw.Amount,
                        CurrencyId = ccyMap[cw.CurrencyId],
                        Memo = cw.Memo,
                        UserId = request.UserId
                    };

                    await mediator.Send(new CreateCashWithdrawal.Command(newCashWithdrawal, request.UserId)).ConfigureAwait(false);
                }
            }

            private async Task DeleteUserData(int userId)
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new NotFoundException($"The user with id {userId} does not exist");
                }

                var transactionsToDelete = await context.Transactions.Where(t => t.UserId == userId).ToListAsync();
                foreach (var trans in transactionsToDelete)
                {
                    await mediator.Send(new DeleteTransaction.Command(trans.Id, userId)).ConfigureAwait(false);
                }

                var keywords = await context.Keywords.Where(k => k.UserId == userId).ToArrayAsync();
                context.Keywords.RemoveRange(keywords);

                var cashWithdrawals = await context.CashWithdrawals.Where(c => c.UserId == userId).ToArrayAsync();
                context.CashWithdrawals.RemoveRange(cashWithdrawals);

                var locations = await context.Locations.Where(l => l.UserId == userId).ToArrayAsync();
                context.Locations.RemoveRange(locations);

                var categories = await context.Categories.Where(c => c.UserId == userId).ToArrayAsync();
                context.Categories.RemoveRange(categories);

                await context.SaveChangesAsync();
            }
        }

        //public class Validator : AbstractValidator<Command>
        //{
        //    public Validator()
        //    {
        //        RuleFor(x => x.TransactionIn).NotNull().DependentRules(() =>
        //        {
        //            RuleFor(c => c.TransactionIn.TransDate).Must(ParseAsDate);
        //            RuleFor(c => c.TransactionIn.UserId).Equal(c => c.TokenUserId);
        //            RuleFor(c => c.TransactionIn.LocationId).GreaterThan(0);
        //            RuleFor(c => c.TransactionIn.CurrencyId).GreaterThan(0);
        //            RuleFor(c => c.TransactionIn.CategoryId).GreaterThan(0);
        //            RuleFor(c => c.TransactionIn.Title).NotEmpty().Length(3, 255);
        //            RuleForEach(c => c.TransactionIn.KeywordIds).GreaterThan(0);
        //        });
        //    }

        //    private bool ParseAsDate(string dateString)
        //    {
        //        return DateTime.TryParseExact(
        //            dateString,
        //            DateStringFormat,
        //            CultureInfo.InvariantCulture,
        //            DateTimeStyles.None,
        //            out var result);
        //    }
        //}
    }
}
