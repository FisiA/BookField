using AutoMapper;
using BookPlace.Core.Domain.Entities;
using BookPlace.Core.DTO;

namespace BookPlace.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping between real Domain Models and DTOs
            CreateMap<Reservation, ReservationDTO>().ReverseMap();
        }
    }
}
