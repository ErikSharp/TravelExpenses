using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
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

        public UsersControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CanAuthenticateUser()
        {
            var email = @"erik.sharp@hadleyshope.com";
            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync(
                "/api/users/authenticate",
                new StringContent(JsonConvert.SerializeObject(new UserIn(email, "password")), Encoding.UTF8, "application/json")
                ).ConfigureAwait(false);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var authenticatedUser = JsonConvert.DeserializeObject<UserOut>(stringResponse);

            authenticatedUser.Email.ShouldBe(email);
            authenticatedUser.Token.ShouldNotBeNullOrEmpty();
        }
    }
}
