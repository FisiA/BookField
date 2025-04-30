using AutoMapper;
using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.Domain.Entities;
using BookPlace.Core.DTO.User;
using BookPlace.Infrastructure.Data;
using BookPlace.Infrastructure.Services;
using BookPlace.IntegrationTests.Utils.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookPlace.IntegrationTests.Services
{
    public class UsersServiceTests : IClassFixture<UsersDbFixture>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersServiceTests(UsersDbFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _mapper = fixture.Mapper;
            _userManager = fixture.UserManager;
            _signInManager = fixture.SignInManager;
            _usersService = new UsersService(_dbContext, _mapper, _userManager, _signInManager);
        }

        [Fact]
        public async Task FindByUsernameAsync_ShouldReturnUserDetailsIfUsernameFound()
        {
            // Arrange
            var username = "admin";

            // Act
            var result = await _usersService.FindByUsernameAsync(username);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task FindByUsernameAsync_ShouldReturnNullIfUsernameNotFound()
        {
            // Arrange
            string username = "testuser";

            // Act
            var result = await _usersService.FindByUsernameAsync(username);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuceededTrueWhenUserDataIsValidAndUserIsCreated()
        {
            // Arrange
            var registerDto = new RegisterUserDTO
            {
                UserName = "batushaasllani",
                Password = "P@ssword!123",
                Email = "batusha.asllani@mailinator.com",
                Name = "Batusha",
                Surname = "Asllani"
            };

            // Act
            var result = await _usersService.CreateUserAsync(registerDto);

            // Assert
            result.Succeeded.Should().BeTrue();

            var createdUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == registerDto.UserName);
            createdUser.Should().NotBeNull();
            createdUser.Email.Should().Be(registerDto.Email);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuccededFalseWhenUserDataIsInvalid()
        {
            // Arrange
            var registerDto = new RegisterUserDTO
            {
                UserName = "testasllani",
                Password = "123",
                Email = "test.asllani@mailinator.com",
                Name = "Test",
                Surname = "Asllani"
            };

            // Act
            var result = await _usersService.CreateUserAsync(registerDto);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CheckSignInAsync_ShouldSucceedWhenCredsAreCorrect()
        {
            // Arrange
            var username = "admin";
            var userFromDb = await _userManager.FindByNameAsync(username);

            // Act
            var signInResult = await _usersService.CheckSignInAsync(userFromDb, "P@ssw0rd@123");

            // Assert
            signInResult.Should().NotBeNull();
            signInResult.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task CheckSignInAsync_ShouldNotSucceedWhenCredsAreIncorrect()
        {
            // Arrange
            var username = "admin";
            var userFromDb = await _userManager.FindByNameAsync(username);

            // Act
            var signInResult = await _usersService.CheckSignInAsync(userFromDb, "Test@1234");

            // Assert
            signInResult.Should().NotBeNull();
            signInResult.Succeeded.Should().BeFalse();
        }
    }
}
