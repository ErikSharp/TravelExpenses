using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.WebAPI;
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

            authenticatedUser.Email.ShouldBe(email, StringCompareShould.IgnoreCase);
            authenticatedUser.Token.ShouldNotBeNullOrEmpty();
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
            var response = JsonConvert.DeserializeObject<FailureResponse>(stringResponse);
            response.Message.ShouldBe("Username or password is incorrect");
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

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var authenticatedUser = JsonConvert.DeserializeObject<UserOut>(stringResponse);

            await CanAuthenticateUser(email, password).ConfigureAwait(false);
        }

        //[Fact]
        //public async Task CantCreateUserAsAlreadyExists()
        //{

        //}

        //[Fact]
        //public async Task CreateUserPasswordTooShort()
        //{

        //}

        //[Fact]
        //public async Task CreateUserPasswordTooLong()
        //{

        //}

        //[Fact]
        //public async Task CreateUserMissingEmail()
        //{

        //}

        //[Fact]
        //public async Task CreateUserMissingPassword()
        //{

        //}

        //[Fact]
        //public async Task CreateUserNotAValidEmailAddress()
        //{

        //}
    }
}
