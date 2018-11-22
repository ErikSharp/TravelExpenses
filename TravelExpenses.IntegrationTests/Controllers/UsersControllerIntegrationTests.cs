﻿using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.WebAPI;
using TravelExpenses.WebAPI.Controllers;
using TravelExpenses.WebAPI.Models;
using Xunit;

namespace TravelExpenses.IntegrationTests.Controllers
{
    public class UsersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        const string AuthPath = "/api/users/authenticate";
        const string CreateUserPath = "/api/users";

        public UsersControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        private StringContent CreateRequestUser(string email, string password)
        {
            return new StringContent(
                JsonConvert.SerializeObject(
                    new UserIn(email, password)), 
                    Encoding.UTF8, 
                    "application/json");
        }

        [Theory]
        [InlineData(SeedData.Email1, "password")]
        [InlineData("ERIK.SHARP@HADLEYSHOPE.COM", "password")]
        public async Task CanAuthenticateUserTests(string email, string password)
        {
            await CanAuthenticateUser(email, password);
        }

        private async Task CanAuthenticateUser(string email, string password)
        {
            var httpResponse = await _client.PutAsync(
                AuthPath,
                CreateRequestUser(email, password)).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var authenticatedUser = JsonConvert.DeserializeObject<UserOut>(stringResponse);

            authenticatedUser.ShouldSatisfyAllConditions(
                () => authenticatedUser.Email.ShouldBe(email, StringCompareShould.IgnoreCase),
                () => authenticatedUser.Token.ShouldNotBeNullOrEmpty()
            );
        }

        [Fact]
        public async Task AuthFailsWithNoBody()
        {
            var httpResponse = await _client.PutAsync(
                AuthPath,
                null).ConfigureAwait(false);

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
        }

        [Theory]
        [InlineData(SeedData.Email1, "wrongpassword")]
        [InlineData("dontknowme@gmail.com", "password")]
        [InlineData(null, "password")]
        [InlineData(SeedData.Email1, null)]
        public async Task AuthBadRequestTests(string email, string password)
        {
            var httpResponse = await _client.PutAsync(
                AuthPath,
                CreateRequestUser(email, password)).ConfigureAwait(false);

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Message.ShouldBe(UsersController.InvalidCredsMsg);
        }

        [Fact]
        public async Task CanCreateUser()
        {
            var email = "lynseysharp@yahoo.com";
            var password = "littlepiggy";

            var httpResponse = await _client.PostAsync(
                CreateUserPath,
                CreateRequestUser(email, password)).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            await CanAuthenticateUser(email, password).ConfigureAwait(false);
        }

        [Fact]
        public async Task CantCreateUserAsAlreadyExists()
        {
            var httpResponse = await _client.PostAsync(
                CreateUserPath,
                CreateRequestUser(SeedData.Email1, "somepassword")).ConfigureAwait(false);

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.Conflict);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Message.ShouldBe(UserAlreadyExistsException.ExMessage);
        }

        [Theory]
        [InlineData(SeedData.Email1, "short")]
        [InlineData(SeedData.Email1, "TooLongWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW")]
        [InlineData("notanemail", "password")]
        [InlineData(null, "password")]
        [InlineData(SeedData.Email1, null)]
        public async Task CreateUserValidationFailureTests(string email, string password)
        {
            var httpResponse = await _client.PostAsync(
                CreateUserPath,
                CreateRequestUser(email, password)).ConfigureAwait(false);

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Message.ShouldContain("ValidationException");
        }
    }
}
