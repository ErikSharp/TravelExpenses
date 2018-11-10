using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TravelExpenses.Application.Features;
using TravelExpenses.Application.Helpers;
using TravelExpenses.Domain.Entities;
using Xunit;
using static TravelExpenses.Application.Features.GetAuthenticatedUser;

namespace TravelExpenses.Application.Tests.Features
{
    public class GetAuthenticatedUserTests
    {
        [Fact]
        public void ValidatorShouldRejectMissingLoginDetails()
        {
            RunValidator(null, "LoginDetails");
        }

        [Fact]
        public void ValidatorShouldRejectInvalidEmail()
        {
            var loginDetails = new UserIn("NotAnEmail", "password");
            RunValidator(loginDetails, "LoginDetails.Email");
        }

        [Fact]
        public void ValidatorShouldRejectShortPassword()
        {
            var loginDetails = new UserIn("erik.sharp@hadleyshope.com", "short");
            RunValidator(loginDetails, "LoginDetails.Password");
        }

        [Fact]
        public void ValidatorShouldRejectLongPassword()
        {
            var loginDetails = new UserIn("erik.sharp@hadleyshope.com", new string('w', 51));
            RunValidator(loginDetails, "LoginDetails.Password");
        }

        private void RunValidator(UserIn loginDetails, string propertyName)
        {
            var validator = new Validator();
            var validation = validator.Validate(new Query(loginDetails));

            validation.Errors.Count.ShouldBe(1);
            validation.Errors.Single().PropertyName.ShouldBe(propertyName);
        }

        [Fact]
        public async void ShouldCreateAuthenticatedUserForCorrectCredentials()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserOut>());
            var mapper = config.CreateMapper();
            var loginDetails = new UserIn(
                "erik.sharp@hadleyshope.com",
                "password");
            var optionsMock = new Mock<IOptions<AppSettings>>();
            optionsMock.Setup(opt => opt.Value).Returns(new AppSettings { Secret = Guid.NewGuid().ToString() });

            var sut = new Handler(
                optionsMock.Object,
                mapper);

            var authenticatedUser = await sut.Handle(
                new Query(loginDetails), 
                CancellationToken.None);

            authenticatedUser.ShouldNotBeNull();
            authenticatedUser.Id.ShouldBe(1);
            authenticatedUser.Email.ShouldBe(loginDetails.Email);
            authenticatedUser.Token.ShouldNotBeNull();
            authenticatedUser.Token.ShouldNotBe("");
        }
    }
}
