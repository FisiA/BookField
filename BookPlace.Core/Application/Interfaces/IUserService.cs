using BookPlace.Core.DTO.User;
using Microsoft.AspNetCore.Identity;

namespace BookPlace.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> FindByUsernameAsync(string username);
        Task<IdentityResult> CreateUserAsync(RegisterUserDTO user);
        Task<SignInResult> CheckSignInAsync(LoginUserDTO user);
    }
}
