using AutoMapper;
using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.Domain.Entities;
using BookPlace.Core.Domain.Enum;
using BookPlace.Core.DTO;
using BookPlace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookPlace.Infrastructure.Services
{
    public class ReservationsService : BaseService, IReservationsService
    {
        
        public ReservationsService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<ReservationDTO>> GetAllReservationsAsync(ReservationState status)
        {
            var reservations = await _dbContext.Reservations
                                               .Where(r => !r.IsDeleted
                                                        && (status == ReservationState.All 
                                                            || (status == ReservationState.Confirmed && r.IsConfirmed) 
                                                            || (status == ReservationState.Uncofirmed && !r.IsConfirmed))
                                                ).ToListAsync();
            return _mapper.Map<List<ReservationDTO>>(reservations);
        }

        public async Task<ReservationDTO> GetReservationByIdAsync(Guid id)
        {
            var reservation = await _dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if(reservation == null)
            {
                return null;
            }
            return _mapper.Map<ReservationDTO>(reservation);
        }

        public async Task<ReservationDTO> CreateReservationAsync(ReservationDTO reservationDetails)
        {
            var reservation = _mapper.Map<Reservation>(reservationDetails);
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ReservationDTO>(reservation);
        }

        public async Task<ReservationDTO> UpdateReservationAsync(ReservationDTO reservationDetails) 
        {
            var reservation = _mapper.Map<Reservation>(reservationDetails);
            _dbContext.Reservations.Update(reservation);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ReservationDTO>(reservation);
        }

        public async Task<bool> DeleteReservationAsync(Guid id)
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);
            if(reservation == null)
            {
                return false;
            }

            reservation.IsDeleted = true;
            _dbContext.Reservations.Update(reservation);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ConfirmReservationAsync(Guid id)
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return false;
            }

            reservation.IsConfirmed = true;
            _dbContext.Reservations.Update(reservation);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
