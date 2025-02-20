using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyApi.Tests
{
    [TestClass]
    public class ProgramTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task GetWeatherForecast_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/tt");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task PostLead_ValidData_ReturnsSuccess()
        {
            // Arrange
            var newLead = new
            {
                Name = "John Doe",
                PhoneNumber = "1234567890",
                ZipCode = "90001",
                CanCommunicate = true,
                Email = "john@example.com"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/leads", newLead);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task PostLead_MissingName_ReturnsBadRequest()
        {
            // Arrange
            var invalidLead = new
            {
                PhoneNumber = "1234567890",
                ZipCode = "90001",
                CanCommunicate = true
            };

            // Act
            var response = await _client.PostAsJsonAsync("/leads", invalidLead);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
