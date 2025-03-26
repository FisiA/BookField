using BookPlace.API.Controllers;
using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.DTO.User;
using BookPlace.UnitTests.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using aspNetCoreIdentity = Microsoft.AspNetCore.Identity;

namespace BookPlace.UnitTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UsersController _userController;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _configurationMock = new Mock<IConfiguration>();
            _userController = new UsersController(_userServiceMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WithUserCreationMessageString()
        {
            // Arrange
            var newUserDetails = UserStaticData.GetRegisterUserDtoDetails();
            _userServiceMock.Setup(s => s.CreateUserAsync(newUserDetails)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userController.Register(newUserDetails);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            var returnValue = okObjectResult.Value;
            Assert.NotNull(returnValue);
            Assert.Equal("User Created Successfully", returnValue);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WithIdentityErrorList()
        {
            // Arrange
            var newUserDetails = UserStaticData.GetRegisterUserDtoDetails();
            var identityRes = IdentityResult.Failed(new IdentityError { Description = "User could not be created" });
            _userServiceMock.Setup(s => s.CreateUserAsync(newUserDetails)).ReturnsAsync(identityRes);

            // Act
            var result = await _userController.Register(newUserDetails);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal(identityRes.Errors, badRequestResult.Value);
        }
                
        [Fact]
        public async Task Login_ReturnsUnathorized_WhenUsernameNotFound()
        {
            // Arrange
            var loginUserDto = UserStaticData.GetLoginUserDtoDetails();
            _userServiceMock.Setup(s => s.FindByUsernameAsync(loginUserDto.UserName)).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _userController.Login(loginUserDto);

            // Assert
            var unathorizedResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unathorizedResult.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsUnathorized_WhenUsernameFoundButPasswordWrong()
        {
            // Arrange
            var loginUserDto = UserStaticData.GetLoginUserDtoDetails();
            var userFound = UserStaticData.GetUsers().Find(u => u.UserName == loginUserDto.UserName);
            _userServiceMock.Setup(s => s.FindByUsernameAsync(loginUserDto.UserName)).ReturnsAsync(userFound);
            _userServiceMock.Setup(s => s.CheckSignInAsync(loginUserDto)).ReturnsAsync(aspNetCoreIdentity.SignInResult.Failed);

            // Act
            var result = await _userController.Login(loginUserDto);

            // Assert
            var unathorizedResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unathorizedResult.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WithJWTToken()
        {
            // Arrange
            var loginUserDto = UserStaticData.GetLoginUserDtoDetails();
            var userFound = UserStaticData.GetUsers().Find(u => u.UserName == loginUserDto.UserName);
            _userServiceMock.Setup(s => s.FindByUsernameAsync(loginUserDto.UserName)).ReturnsAsync(userFound);
            _userServiceMock.Setup(s => s.CheckSignInAsync(loginUserDto)).ReturnsAsync(aspNetCoreIdentity.SignInResult.Success);
            _configurationMock.Setup(c => c["Jwt:SecurityKey"]).Returns("dd459b848f43a51f2f99d05736c3db6eebab2294459f82025578126cfd5649b86b737735f6a2be072b6606966e7afe4525bec06c0cf9a4bd6493cb6ee17934a9");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("BookPlace");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("BookPlace-Users");

            // Act
            var result = await _userController.Login(loginUserDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            var returnValue = okObjectResult.Value;
            Assert.NotNull(returnValue);
            Assert.IsType<string>(returnValue);
        }

    }
}
