using AutoMapper;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.CashWithdrawals;
using TravelExpenses.Application.Features.Categories;
using TravelExpenses.Application.Features.Keywords;
using TravelExpenses.Application.Features.Locations;
using TravelExpenses.Application.Features.Transactions;
using TravelExpenses.Application.Features.Users;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Utilities
{
    public class SynthesizeUser
    {
        public class Command : IRequest
        {
            public Command(string randomEmail)
            {
                RandomEmail = randomEmail;
            }

            public string RandomEmail { get; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TravelExpensesContext context;
            private readonly IMediator mediator;
            const string ipsum = "Distillery VHS austin portland hammock, food truck normcore pok pok disrupt migas poke. Literally tbh fixie, chicharrones marfa trust fund normcore tofu vice put a bird on it asymmetrical offal. Cornhole scenester pitchfork twee whatever meggings four loko. Blue bottle farm-to-table before they sold out four loko, schlitz af pickled keytar copper mug XOXO mlkshk twee tilde humblebrag post-ironic. Chartreuse fixie poke viral biodiesel master cleanse mustache.";

            public Handler(
                TravelExpensesContext context,
                IMediator mediator)
            {
                this.context = context;
                this.mediator = mediator;
            }

            protected async override Task Handle(Command request, CancellationToken cancellationToken)
            {
                var reg = new UserRegistration
                {
                    Username = "ManBearPig",
                    Email = request.RandomEmail,
                    Password = "123456"
                };

                var user = await mediator.Send(new CreateUser.Command(reg)).ConfigureAwait(false);
                var userId = context.Users.Single(u => u.Email == user.Email).Id;

                // create categories

                var catNames = new []
                {
                    "Transportation",
                    "Dining",
                    "Groceries",
                    "Entertainment",
                    "Accommodations",
                    "Utilities",
                    "Medical",
                    "Fees",
                    "Deposit",
                    "Non-trip",
                    "Loss/Gain"
                };

                var categories = await mediator.Send(new CreateCategory.Query(
                    catNames.Select(cn => 
                        new Category { CategoryName = cn, UserId = userId }).ToArray()
                )).ConfigureAwait(false);

                var keywordNames = new []
                {
                    "Expensive",
                    "Cheap",
                    "Coffee",
                    "Alcohol",
                    "Lunch",
                    "Dinner",
                    "Breakfast",
                    "Uber",
                    "Doctor",
                    "Dentist"
                };

                var keywords = await mediator.Send(new CreateKeyword.Query(keywordNames, userId)).ConfigureAwait(false);

                var countries = context.Countries
                    .Where(c => 
                        new [] {
                            "Thailand",
                            "Mexico",
                            "Viet Nam",
                            "United States",
                            "Japan"
                        }.Contains(c.CountryName)).ToArray();

                LocationOut[] locations = new LocationOut[0];

                foreach (var country in countries)
                {
                    switch(country.CountryName)
                    {
                        case "Thailand":
                            locations = await CreateLocation(country.Id, "Chiang Mai").ConfigureAwait(false);
                            locations = await CreateLocation(country.Id, "Bangkok").ConfigureAwait(false);
                            locations = await CreateLocation(country.Id, "Koh Tao").ConfigureAwait(false);
                            break;
                        case "Mexico":
                            locations = await CreateLocation(country.Id, "Guadalajara").ConfigureAwait(false);
                            locations = await CreateLocation(country.Id, "Merida").ConfigureAwait(false);
                            locations = await CreateLocation(country.Id, "Playa del Carmen").ConfigureAwait(false);
                            break;
                        case "Viet Nam":
                            locations = await CreateLocation(country.Id, "Hanoi").ConfigureAwait(false);
                            locations = await CreateLocation(country.Id, "Ho Chi Minh City").ConfigureAwait(false);
                            break;
                        case "United States":
                            locations = await CreateLocation(country.Id, "California").ConfigureAwait(false);
                            locations = await CreateLocation(country.Id, "New York").ConfigureAwait(false);
                            break;
                        case "Japan":
                            locations = await CreateLocation(country.Id, "Tokyo").ConfigureAwait(false);
                            break;
                    }
                }

                var currencyIds = context.Currencies.Select(c => c.Id).ToArray();

                var titles = context.Transactions
                    .OrderBy(t => Guid.NewGuid())
                    .Select(t => t.Title)
                    .Take(100)
                    .ToArray();

                var transactionCount = 200;
                var dayCounter = 0;
                var rand = new Random();
                for (int i = 0; i < transactionCount; i++)
                {
                    dayCounter = OneIn(3) ? dayCounter - rand.Next(1, 4) : dayCounter;
                    var trans = new TransactionCreateIn
                    {
                        TransDate = DateTime.Today.AddDays(dayCounter).ToString("yyyy-MM-dd"),
                        Amount = rand.Next(5, 500),
                        LocationId = locations.OrderBy(l => Guid.NewGuid()).First().Id,
                        CurrencyId = currencyIds.OrderBy(c => Guid.NewGuid()).First(),
                        CategoryId = categories.OrderBy(c => Guid.NewGuid()).First().Id,
                        Title = titles.OrderBy(t => Guid.NewGuid()).First(),
                        Memo = OneIn(4) ? ipsum.Substring(0, (int)(ipsum.Length * rand.NextDouble())) : null,
                        PaidWithCash = OneIn(6) ? false : true,
                        UserId = userId,
                        KeywordIds = OneIn(3) ? 
                            keywords.OrderBy(k => Guid.NewGuid())
                                .Take(rand.Next(1, 4))
                                .Select(k => k.Id)
                                .ToArray() 
                            : null
                    };
                    await mediator.Send(new CreateTransaction.Command(trans, userId)).ConfigureAwait(false);
                }

                var withdrawalTitles = context.CashWithdrawals
                    .OrderBy(t => Guid.NewGuid())
                    .Select(t => t.Title)
                    .Take(100)
                    .ToArray();

                var withdrawalCount = 20;
                dayCounter = 0;
                for (int i = 0; i < withdrawalCount; i++)
                {
                    dayCounter = OneIn(3) ? dayCounter - rand.Next(1, 4) : dayCounter;

                    var withdrawal = new CashWithdrawalCreateIn
                    {
                        Title = withdrawalTitles.OrderBy(t => Guid.NewGuid()).First(),
                        TransDate = DateTime.Today.AddDays(dayCounter).ToString("yyyy-MM-dd"),
                        Amount = rand.Next(1, 10) * 100,
                        CurrencyId = currencyIds.OrderBy(c => Guid.NewGuid()).First(),
                        LocationId = locations.OrderBy(l => Guid.NewGuid()).First().Id,
                        Memo = OneIn(4) ? ipsum.Substring(0, (int)(ipsum.Length * rand.NextDouble())) : null,
                        UserId = userId
                    };

                    await mediator.Send(new CreateCashWithdrawal.Command(withdrawal, userId)).ConfigureAwait(false);
                }

                bool OneIn(int x)
                {
                    return rand.Next() % x == 0;
                }

                Task<LocationOut[]> CreateLocation(int countryId, string locationName)
                {
                    var location = new Location
                    {
                        CountryId = countryId,
                        LocationName = locationName,
                        UserId = userId
                    };

                    return mediator.Send(new CreateLocation.Query(location));
                }

                Log.Information("Finished creating user {User}", reg);
            }
        }
    }
}
