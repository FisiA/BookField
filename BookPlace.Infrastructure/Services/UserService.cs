﻿using AutoMapper;
using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.Domain.Entities;
using BookPlace.Core.DTO.User;
using BookPlace.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace BookPlace.Infrastructure.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(AppDbContext dbContext, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) : base(dbContext, mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;            
        }

        public async Task<UserDTO> FindByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }

            return null;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterUserDTO user)
        {
            return await _userManager.CreateAsync(_mapper.Map<User>(user), user.Password);
        }

        public async Task<SignInResult> CheckSignInAsync(LoginUserDTO user)
        {
            return await _signInManager.CheckPasswordSignInAsync(_mapper.Map<User>(user), user.Password, false);
        }
    }
}
