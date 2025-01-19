using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http.Json;

namespace NewsagentTest
{
    public class NewsagentApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public NewsagentApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ValidateSingle_ShouldReturn_ValidResult()
        {
            // Arrange
            var request = new
            {
                Name = "Sydney Central News",
                ChainId = "SUP",
                Address1 = "123 George St",
                City = "Sydney",
                State = "NSW",
                PostCode = "2000",
                Latitude = -33.859733605046706, 
                Longitude = 151.2082647031474
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/newsagent/validate", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var result = await response.Content.ReadFromJsonAsync<ValidationResult>();
            result.Should().NotBeNull();
            result!.IsValid.Should().BeTrue();
            result.Message.Should().BeNull();
        }

        [Fact]
        public async Task ValidateByName_ShouldReturn_ValidResult()
        {
            // Arrange
            var name = "Sydney Central News";

            // Act
            var response = await _client.GetAsync($"/api/newsagent/validate/by-name/{name}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var result = await response.Content.ReadFromJsonAsync<object>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ValidateAll_ShouldReturn_ResultsForChain()
        {
            // Arrange
            var chainId = "SUP";

            // Act
            var response = await _client.GetAsync($"/api/newsagent/validate/{chainId}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var results = await response.Content.ReadFromJsonAsync<List<ValidationResult>>();
            results.Should().NotBeNull();
            results!.Count.Should().BeGreaterThan(0);
            results[0].IsValid.Should().BeTrue();
        }
    }

    public record ValidationResult(bool IsValid, string? Message);
}