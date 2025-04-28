using BookPlace.Core.Domain.Enum;
using BookPlace.Core.DTO.Reservation;
using BookPlace.IntegrationTests.Utils;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace BookPlace.IntegrationTests.Controllers
{
    public class ReservationsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseURL = "/v1/Reservations";

        public ReservationsControllerTests(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Theory]
        [InlineData(ReservationState.All)]
        [InlineData(ReservationState.Confirmed)]
        [InlineData(ReservationState.Uncofirmed)]

        public async Task GetAllReservations_ShouldReturnOkWithListOfReservationsPerStatus(ReservationState reservationState) 
        {
            // Act
            var response = await _httpClient.GetAsync($"{_baseURL}/GetAll/{reservationState}");

            // Assert
            // Using FluentAssertions to be more elegant and clean
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var reservationList = await response.Content.ReadFromJsonAsync<List<ReservationDTO>>();
            reservationList.Should().NotBeNull();
        }

        [Fact]
        public async Task GetReservationById_WithNonExistentId_ShouldReturnNotFound()
        {
            // Arrange
            var reservationId = Guid.NewGuid();

            // Act
            var response = await _httpClient.GetAsync($"{_baseURL}/GetById/{reservationId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetReservationById_WithExistentId_ShouldReturnOk()
        {
            // Arrange
            var reservation = new ReservationDTO
            {
                Id = Guid.Parse("17a8e4ba-29fd-414e-9302-6a62b55a28af")
            };

            // Act
            var response = await _httpClient.GetAsync($"{_baseURL}/GetById/{reservation.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var reservationDto = await response.Content.ReadFromJsonAsync<ReservationDTO>();
            reservationDto.Should().NotBeNull();
            reservationDto.Id.Should().Be(reservation.Id);
        }

        [Fact]
        public async Task CreateReservation_ShouldReturnOkWithNewReservation()
        {
            // Arrange
            var reservation = new ReservationDTO
            {
                Id = Guid.Parse("5d9c5d1f-4ac2-4cb5-b3f4-c456e3a1e98e"),
                NameAndSurname = "Lartan Fakaj",
                Email = "lartan.fakaj@mailinator.com",
                ReservationFrom = new DateTime(2025, 4, 30, 14, 0, 0),
                ReservationTo = new DateTime(2025, 4, 30, 15, 0, 0),
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseURL}", reservation);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var createdReservation = await response.Content.ReadFromJsonAsync<ReservationDTO>();
            createdReservation.Should().NotBeNull();
            createdReservation.Id.Should().Be(reservation.Id);
        }

        [Fact]
        public async Task UpdateReservation_WithIdMismatch_ShouldReturnBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var reservation = new ReservationDTO
            {
                Id = Guid.NewGuid(), // different ID
                NameAndSurname = "Filan Fisteku",
                Email = "filan.fisteku@mailinator.com",
                ReservationFrom = new DateTime(2025, 04, 30, 14, 0, 0),
                ReservationTo = new DateTime(2025, 04, 30, 15, 0, 0),
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync($"{_baseURL}/{id}", reservation);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateReservation_WithNonExistentId_ShouldReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var reservation = new ReservationDTO
            {
                Id = id, // same ID
                NameAndSurname = "Cicero Boja",
                Email = "cicero.boja@mailinator.com",
                ReservationFrom = new DateTime(2025, 06, 30, 14, 0, 0),
                ReservationTo = new DateTime(2025, 06, 30, 15, 0, 0),
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync($"{_baseURL}/{id}", reservation);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateReservation_WithExistentId_ShouldReturnOk()
        {
            // Arrange
            var id = Guid.Parse("17a8e4ba-29fd-414e-9302-6a62b55a28af");
            var reservation = new ReservationDTO
            {
                Id = id, // same ID
                NameAndSurname = "Laidan Broci",
                Email = "laidan.broci01@mailinator.com",
                ReservationFrom = new DateTime(2025, 4, 30, 14, 0, 0),
                ReservationTo = new DateTime(2025, 4, 30, 15, 0, 0)
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync($"{_baseURL}/{id}", reservation);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedReservation = await response.Content.ReadFromJsonAsync<ReservationDTO>();
            updatedReservation.Should().NotBeNull();
        }

        [Theory]
        [InlineData("25a8e4ca-31fd-444e-9152-6a30b55a38af")] // Nonexisting ID
        [InlineData("986c0ee0-a114-4778-a1c3-845879952432")] // Existing ID
        public async Task ConfirmReservation_ShouldReturnOkWithConfirmationStatus(string id)
        {
            // Arrange
            var idToConfirm = Guid.Parse(id);
            // If we are trying to confirm the existing Id status will be true, otherwise it will always be false
            var confirmationStatus = id.Equals("986c0ee0-a114-4778-a1c3-845879952432");

            // Act
            var response = await _httpClient.PutAsync($"{_baseURL}/ConfirmReservation/{idToConfirm}", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deleteResult = await response.Content.ReadFromJsonAsync<bool>();
            deleteResult.Should().Be(confirmationStatus);
        }

        [Theory]
        [InlineData("25a8e4ca-31fd-444e-9152-6a30b55a38af")] // Nonexisting ID
        [InlineData("17a8e4ba-29fd-414e-9302-6a62b55a28af")] // Existing ID
        public async Task DeleteReservation_ShouldReturnOkWithDeletionStatus(string id)
        {
            // Arrange
            var idToDelete = Guid.Parse(id);
            // If we are trying to confirm the existing Id status will be true, otherwise it will always be false
            var deletionStatus = id.Equals("17a8e4ba-29fd-414e-9302-6a62b55a28af");

            // Act
            var response = await _httpClient.DeleteAsync($"{_baseURL}/{idToDelete}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deleteResult = await response.Content.ReadFromJsonAsync<bool>();
            deleteResult.Should().Be(deletionStatus);
        }
    }
}