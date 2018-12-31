using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Helpers;
using TravelExpenses.Application.Interfaces;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Infrastructure;
using TravelExpenses.Persistence;
using Xunit;
using static TravelExpenses.Application.Features.Users.GetAuthenticatedUser;

namespace TravelExpenses.Application.Tests.Features
{
    public class GetAuthenticatedUserTests
    {
        [Fact]
        public async void ShouldCreateAuthenticatedUserForCorrectCredentials()
        {
            var options = new DbContextOptionsBuilder<TravelExpensesContext>()
                .UseInMemoryDatabase(databaseName: nameof(ShouldCreateAuthenticatedUserForCorrectCredentials))
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new TravelExpensesContext(options))
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Email = "erik.sharp@hadleyshope.com",
                    PasswordHash = "$2y$12$yVYkJsR7a4Wj3wRzCD9Pn.DvDGWY3Dzx2AwisSqailn3Pyu.X.zWi" //password
                });
                context.SaveChanges();
            }

            using (var context = new TravelExpensesContext(options))
            {
                var loginDetails = new UserIn(
                    "CaptainBedpan",
                    "erik.sharp@hadleyshope.com",
                    "password");

                var sut = CreateHandler(loginDetails, context);

                var authenticatedUser = await sut.Handle(
                    new Query(loginDetails),
                    CancellationToken.None);

                authenticatedUser.ShouldNotBeNull();
                authenticatedUser.Email.ShouldBe(loginDetails.Email);
                authenticatedUser.Token.ShouldNotBeNull();
                authenticatedUser.Token.ShouldNotBe("");
            }
        }

        [Fact]
        public async void ShouldNotCreateAuthenticatedUserWhenUserDisabled()
        {
            var options = new DbContextOptionsBuilder<TravelExpensesContext>()
                .UseInMemoryDatabase(databaseName: nameof(ShouldNotCreateAuthenticatedUserWhenUserDisabled))
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new TravelExpensesContext(options))
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Email = "erik.sharp@hadleyshope.com",
                    PasswordHash = "$2y$12$yVYkJsR7a4Wj3wRzCD9Pn.DvDGWY3Dzx2AwisSqailn3Pyu.X.zWi", //password
                    Disabled = true
                });
                context.SaveChanges();
            }

            using (var context = new TravelExpensesContext(options))
            {
                var loginDetails = new UserIn(
                    "CaptainBedpan",
                    "erik.sharp@hadleyshope.com",
                    "password");

                var sut = CreateHandler(loginDetails, context);

                var authenticatedUser = await sut.Handle(
                    new Query(loginDetails),
                    CancellationToken.None);

                authenticatedUser.ShouldBeNull();
            }
        }

        private Handler CreateHandler(UserIn loginDetails, TravelExpensesContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserOut>());
            var mapper = config.CreateMapper();

            var tokenGenerator = new Mock<ITokenGenerator>();
            tokenGenerator.Setup(tg => tg.CreateTokenString(It.IsAny<User>())).Returns("token");
            
            var loggerMock = new Mock<ILogger>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(fac => fac.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

            return new Handler(
                context,
                mapper,
                tokenGenerator.Object,
                loggerFactoryMock.Object);
        }
    }
}
