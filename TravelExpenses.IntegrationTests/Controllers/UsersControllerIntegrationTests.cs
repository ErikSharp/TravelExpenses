using Newtonsoft.Json;
using Shouldly;
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

        private StringContent CreateRequestUser(string username, string email, string password)
        {
            return new StringContent(
                JsonConvert.SerializeObject(
                    new UserIn(username, email, password)), 
                    Encoding.UTF8, 
                    "application/json");
        }

        [Theory]
        [InlineData("CaptainBedpan", SeedData.Email1, "password")]
        [InlineData("CaptainBedpan", "ERIK.SHARP@HADLEYSHOPE.COM", "password")]
        public async Task CanAuthenticateUserTests(string username, string email, string password)
        {
            await CanAuthenticateUser(username, email, password);
        }

        private async Task CanAuthenticateUser(string username, string email, string password)
        {
            var httpResponse = await _client.PutAsync(
                AuthPath,
                CreateRequestUser(username, email, password)).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var authenticatedUser = JsonConvert.DeserializeObject<AuthenticatedUserOut>(stringResponse);

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
        [InlineData("CaptainBedpan", SeedData.Email1, "wrongpassword")]
        [InlineData("CaptainBedpan", "dontknowme@gmail.com", "password")]
        [InlineData("CaptainBedpan", null, "password")]
        [InlineData("CaptainBedpan", SeedData.Email1, null)]
        public async Task AuthBadRequestTests(string username, string email, string password)
        {
            var httpResponse = await _client.PutAsync(
                AuthPath,
                CreateRequestUser(username, email, password)).ConfigureAwait(false);

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Message.ShouldBe(UsersController.InvalidCredsMsg);
        }

        [Fact]
        public async Task CanCreateUser()
        {
            var username = "sugarbooger";
            var email = "lynseysharp@yahoo.com";
            var password = "littlepiggy";

            var httpResponse = await _client.PostAsync(
                CreateUserPath,
                CreateRequestUser(username, email, password)).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            await CanAuthenticateUser(username, email, password).ConfigureAwait(false);
        }

        [Fact]
        public async Task CantCreateUserAsAlreadyExists()
        {
            var httpResponse = await _client.PostAsync(
                CreateUserPath,
                CreateRequestUser("CaptainBedpan", SeedData.Email1, "somepassword")).ConfigureAwait(false);

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.Conflict);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Message.ShouldBe(UserAlreadyExistsException.ExMessage);
        }

        [Theory]
        [InlineData("CaptainBedpan", SeedData.Email1, "short")]
        [InlineData("CaptainBedpan", SeedData.Email1, "TooLongWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW")]
        [InlineData("CaptainBedpan", "notanemail", "password")]
        [InlineData("CaptainBedpan", null, "password")]
        [InlineData("CaptainBedpan", SeedData.Email1, null)]
        public async Task CreateUserValidationFailureTests(string username, string email, string password)
        {
            var httpResponse = await _client.PostAsync(
                CreateUserPath,
                CreateRequestUser(username, email, password)).ConfigureAwait(false);

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Message.ShouldContain("ValidationException");
        }
    }
}
