using BookPlace.Core.Domain.Entities;
using BookPlace.Core.DTO.User;
using Microsoft.AspNetCore.Identity;

namespace BookPlace.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> FindByUsernameAsync(string username);
        Task<IdentityResult> CreateUserAsync(RegisterUserDTO user);
        Task<SignInResult> CheckSignInAsync(User user, string password);
    }
}
