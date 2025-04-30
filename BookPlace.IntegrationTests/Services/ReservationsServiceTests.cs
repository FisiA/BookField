using AutoMapper;
using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.Domain.Enum;
using BookPlace.Core.DTO.Reservation;
using BookPlace.Infrastructure.Data;
using BookPlace.Infrastructure.Services;
using BookPlace.IntegrationTests.Utils;
using FluentAssertions;

namespace BookPlace.IntegrationTests.Services
{
    public class ReservationsServiceTests : IClassFixture<ReservationsDbFixture>
    {
        private readonly AppDbContext _dbContext;
        private readonly IReservationsService _reservationsService;
        private readonly IMapper _mapper;

        public ReservationsServiceTests(ReservationsDbFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _mapper = fixture.Mapper;
            _reservationsService = new ReservationsService(_dbContext, _mapper);
        }

        [Theory]
        [InlineData(ReservationState.All)]
        [InlineData(ReservationState.Uncofirmed)]
        [InlineData(ReservationState.Confirmed)]
        public async Task GetAllReservationsAsync_ShouldReturnOkWithListOfReservationsPerStatus(ReservationState reservationStatus)
        {
            // Act
            var result = await _reservationsService.GetAllReservationsAsync(reservationStatus);

            // Assert
            result.Should().NotBeNull();
            switch (reservationStatus)
            {
                case ReservationState.All:
                    // 3 records
                    result.Should().HaveCount(3);
                    break;
                case ReservationState.Uncofirmed:
                    // 2 records
                    result.Should().HaveCount(2);
                    break;
                default:
                    // 1 record
                    result.Should().HaveCount(1);
                    break;
            }
        }

        [Theory]
        [InlineData("0f43a018-d104-4978-b2df-88ea884cc17c")] // New ID
        [InlineData("47e3921e-6c37-4b34-9e13-148f140deafd")] // Existing ID
        public async Task GetReservationByIdAsync_ShouldReturnReservationOrNullIfNotFound(string id)
        {
            // Arrange
            var reservationId = Guid.Parse(id);
            var existingId = id.Equals("47e3921e-6c37-4b34-9e13-148f140deafd");

            // Act
            var result = await _reservationsService.GetReservationByIdAsync(reservationId);

            // Assert
            if (existingId)
            {
                result.Should().NotBeNull();
                result.Id.Should().Be(reservationId);
            }
            else
            {
                result.Should().BeNull();
            }
        }

        [Fact]
        public async Task CreateReservationAsync_ShouldAddReservation()
        {
            // Arrange
            var newReservation = new ReservationDTO
            {
                Id = Guid.Parse("a32979e5-1fa4-4f22-a1ef-0e227649b7aa"),
                NameAndSurname = "Arif Topalli",
                Email = "arif.topalli@mailinator.com",
                ReservationFrom = DateTime.UtcNow,
                ReservationTo = DateTime.UtcNow.AddHours(1)
            };

            // Act
            var createReservation = await _reservationsService.CreateReservationAsync(newReservation);

            // Assert
            createReservation.Should().NotBeNull();
            createReservation.Id.Should().Be(newReservation.Id);
            createReservation.NameAndSurname.Should().Be(newReservation.NameAndSurname);
            createReservation.Email.Should().Be(newReservation.Email);

            var reservationInDb = await _dbContext.Reservations.FindAsync(createReservation.Id);
            reservationInDb.Should().NotBeNull();
            reservationInDb.NameAndSurname.Should().Be(newReservation.NameAndSurname);
            reservationInDb.Email.Should().Be(newReservation.Email);
        }
        
        [Theory]
        [InlineData("0f43a018-d104-4978-b2df-88ea884cc17c")] // New ID
        [InlineData("47e3921e-6c37-4b34-9e13-148f140deafd")] // Existing ID
        public async Task UpdateReservationAsync_ShouldUpdateFieldsIfReservationExistsOrReturnNull(string id)
        {
            // Arrange
            var updatedDto = new ReservationDTO
            {
                Id = Guid.Parse(id),
                NameAndSurname = "Flladim pula",
                Email = "flladim.pula01@mailinator.com",
                ReservationFrom = DateTime.UtcNow,
                ReservationTo = DateTime.UtcNow.AddHours(2)
            };
            var existingId = id.Equals("47e3921e-6c37-4b34-9e13-148f140deafd");

            // Act
            var updatedReservation = await _reservationsService.UpdateReservationAsync(updatedDto);

            // Assert
            if (existingId)
            {
                updatedReservation.Should().NotBeNull();
                updatedReservation.Email.Should().Be(updatedDto.Email);

                var reservationInDb = await _dbContext.Reservations.FindAsync(updatedReservation.Id);
                reservationInDb.Email.Should().Be(updatedDto.Email);
                reservationInDb.NameAndSurname.Should().Be(updatedDto.NameAndSurname);
            }
            else
            {
                updatedReservation.Should().BeNull();
            }            
        }

        [Theory]
        [InlineData("0f43a018-d104-4978-b2df-88ea884cc17c")] // New ID
        [InlineData("47e3921e-6c37-4b34-9e13-148f140deafd")] // Existing ID
        public async Task DeleteReservationAsync_ShouldReturnTrueIfReservationDeletedOrElseFalse(string id)
        {
            // Arrange
            var reservationId = Guid.Parse(id);
            var existingId = id.Equals("47e3921e-6c37-4b34-9e13-148f140deafd");

            // Act
            var result = await _reservationsService.DeleteReservationAsync(reservationId);

            // Assert
            if (existingId)
            {
                result.Should().BeTrue();

                var deletedReservation = await _dbContext.Reservations.FindAsync(reservationId);
                deletedReservation.IsDeleted.Should().BeTrue();
            }
            else
            {
                result.Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("0f43a018-d104-4978-b2df-88ea884cc17c")] // New ID
        [InlineData("47e3921e-6c37-4b34-9e13-148f140deafd")] // Existing ID
        public async Task ConfirmReservationAsync_ShouldReturnTrueIfReservationConfirmedOrElseFalse(string id)
        {
            // Arrange
            var reservationId = Guid.Parse(id);
            var existingId = id.Equals("47e3921e-6c37-4b34-9e13-148f140deafd");

            // Act
            var result = await _reservationsService.ConfirmReservationAsync(reservationId);

            // Assert
            if (existingId)
            {
                result.Should().BeTrue();

                var updatedReservation = await _dbContext.Reservations.FindAsync(reservationId);
                updatedReservation.IsConfirmed.Should().BeTrue();
            }
            else
            {
                result.Should().BeFalse();
            }
           
        }
    }
}
