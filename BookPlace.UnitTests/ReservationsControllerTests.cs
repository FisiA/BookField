using BookPlace.API.Controllers;
using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.Domain.Enum;
using BookPlace.Core.DTO.Reservation;
using BookPlace.UnitTests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookPlace.UnitTests
{
    public class ReservationsControllerTests
    {
        private readonly Mock<IReservationsService> _mockReservationService;
        private readonly ReservationsController _controller;

        public ReservationsControllerTests()
        {
            _mockReservationService = new Mock<IReservationsService>();
            _controller = new ReservationsController(_mockReservationService.Object);
        }

        [Theory]
        [InlineData(ReservationState.All)]
        [InlineData(ReservationState.Confirmed)]
        [InlineData(ReservationState.Uncofirmed)]
        public async Task GetAllReservationsByStatus_ReturnsOkResult_WithListOfReservations(ReservationState reservationState)
        {
            // Arrange
            var reservations = ReservationStaticData.GetReservations();
            _mockReservationService.Setup(service => service.GetAllReservationsAsync(reservationState)).ReturnsAsync(reservations);

            // Act
            var result = await _controller.GetAllReservations(reservationState);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ReservationDTO>>(okResult.Value);
            Assert.Equal(reservations, returnValue);
        }

        [Fact]
        public async Task GetReservationById_ReturnsOkResult_WithReservation()
        {
            // Arrange
            var reservationId = Guid.Parse("857e93da-688a-42d7-bfc4-1cc1da685770");
            var reservation = ReservationStaticData.GetReservations().FirstOrDefault(r => r.Id.Equals(reservationId));
            _mockReservationService.Setup(service => service.GetReservationByIdAsync(reservationId)).ReturnsAsync(reservation);

            // Act
            var result = await _controller.GetReservationById(reservationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReservationDTO>(okResult.Value);
            Assert.Equal(reservation, returnValue);
        }

        [Fact]
        public async Task GetReservationById_ReturnsNotFound_WhenNoReservationFound()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var reservation = ReservationStaticData.GetReservations().FirstOrDefault(r => r.Id.Equals(reservationId));
            _mockReservationService.Setup(service => service.GetReservationByIdAsync(reservationId)).ReturnsAsync(reservation);

            // Act
            var result = await _controller.GetReservationById(reservationId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateReservation_ReturnsOkResult_WithNewReservation()
        {
            // Arrange
            var reservationDetails = ReservationStaticData.GetReservations().First();
            _mockReservationService.Setup(service => service.CreateReservationAsync(reservationDetails)).ReturnsAsync(reservationDetails);

            // Act
            var result = await _controller.CreateReservation(reservationDetails);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            var returnValue = Assert.IsType<ReservationDTO>(okObjectResult.Value);
            Assert.Equal(reservationDetails, returnValue);
        }

        [Fact]
        public async Task UpdateReservation_ReturnsOkResult_WithUpdatedReservation()
        {
            // Arrange
            var reservationDetails = ReservationStaticData.GetReservations().First();
            _mockReservationService.Setup(service => service.UpdateReservationAsync(reservationDetails)).ReturnsAsync(reservationDetails);

            // Act
            var result = await _controller.UpdateReservation(reservationDetails.Id, reservationDetails);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnValue = Assert.IsType<ReservationDTO>(okResult.Value);
            Assert.Equal(reservationDetails, returnValue);
        }

        [Fact]
        public async Task UpdateReservation_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var reservationDetails = ReservationStaticData.GetReservations().First();
            var differentId = Guid.NewGuid();

            // Act
            var result = await _controller.UpdateReservation(differentId, reservationDetails);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Reservation Id mismatch", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateReservation_ReturnsNotFound_WhenReservationNotFound()
        {
            // Arrange
            var reservationDetails = ReservationStaticData.GetReservations().First();
            _mockReservationService.Setup(service => service.UpdateReservationAsync(reservationDetails)).ReturnsAsync((ReservationDTO)null);

            // Act
            var result = await _controller.UpdateReservation(reservationDetails.Id, reservationDetails);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task ConfirmReservation_ReturnsOkResult_WithConfirmationStatus()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var confirmationStatus = true;
            _mockReservationService.Setup(service => service.ConfirmReservationAsync(reservationId)).ReturnsAsync(confirmationStatus);

            // Act
            var result = await _controller.ConfirmReservation(reservationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.Equal(confirmationStatus, returnValue);
        }

        [Fact]
        public async Task DeleteReservation_ReturnsOkResult_WithDeletionStatus()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var deletionStatus = true;
            _mockReservationService.Setup(service => service.DeleteReservationAsync(reservationId)).ReturnsAsync(deletionStatus);

            // Act
            var result = await _controller.DeleteReservation(reservationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.Equal(deletionStatus, returnValue);
        }
    }
}
