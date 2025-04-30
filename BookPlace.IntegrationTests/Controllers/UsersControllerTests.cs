using BookPlace.Core.DTO.User;
using BookPlace.IntegrationTests.Utils.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Http.Json;

namespace BookPlace.IntegrationTests.Controllers
{
    public class UsersControllerTests : IClassFixture<UsersCustomFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseURL = "/v1/Users";

        public UsersControllerTests(UsersCustomFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturnOkWithCreatedSuccessfullyMessageWhenDataIsValid()
        {
            // Arrange
            var newUserDetails = new RegisterUserDTO
            {
                UserName = "testuser",
                Email = "testuser@mailinator.com",
                Name = "Test",
                Surname = "User",
                Password = "P@ssw0rd@123"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseURL}/Create", newUserDetails);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var resultString = await response.Content.ReadAsStringAsync();
            resultString.Should().Contain("User Created Successfully");
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequestWhenDataIsInvalid()
        {
            // Arrange
            var newUserDetails = new RegisterUserDTO
            {
                UserName = "testuser",
                Email = "testuser@mailinator.com",
                Name = "Test",
                Surname = "User",
                Password = "1" // Pwd too short and not valid
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseURL}/Create", newUserDetails);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorList = await response.Content.ReadFromJsonAsync<List<IdentityError>>();
            errorList.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Login_ShouldReturnOkWithJWTTokenWhenLoginSuccessful()
        {
            // Arrange
            var userLogin = new LoginUserDTO
            {
                UserName = "admin",
                Password = "P@ssw0rd@123"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseURL}/Login", userLogin);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var token = await response.Content.ReadAsStringAsync();
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_ShouldReturnUnathorizedWhenBothCredsWrong()
        {
            // Arrange
            var userLogin = new LoginUserDTO
            {
                UserName = "superadmin1", // Incorrect
                Password = "P@ssw0rd@123" // Incorrect
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseURL}/Login", userLogin);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_ShouldReturnUnathorizedWhenAnyCredsAreWrong()
        {
            // Arrange
            var userLogin = new LoginUserDTO
            {
                UserName = "superadmin", // Correct
                Password = "P@ssw0rd@12345" // Incorrect
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseURL}/Login", userLogin);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
