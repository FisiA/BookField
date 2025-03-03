using BookPlace.Core.Domain.Enum;
using BookPlace.Core.DTO;

namespace BookPlace.Core.Application.Interfaces
{
    public interface IReservationsService
    {
        Task<List<ReservationDTO>> GetAllReservationsAsync(ReservationState status);
        Task<ReservationDTO> GetReservationByIdAsync(Guid id);
        Task<ReservationDTO> CreateReservationAsync(ReservationDTO reservationDetails);
        Task<ReservationDTO> UpdateReservationAsync(ReservationDTO reservationDetails);
        Task<bool> DeleteReservationAsync(Guid id);
        Task<bool> ConfirmReservationAsync(Guid id);
    }
}
