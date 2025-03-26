using AutoMapper;
using BookPlace.Core.Domain.Entities;
using BookPlace.Core.DTO.Reservation;
using BookPlace.Core.DTO.User;

namespace BookPlace.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping between real Domain Models and DTOs
            CreateMap<Reservation, ReservationDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, RegisterUserDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
        }
    }
}
