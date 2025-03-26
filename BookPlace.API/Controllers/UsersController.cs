using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookPlace.API.Controllers
{
    [ApiController]
    [Route("/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO model) 
        {
            var result = await _userService.CreateUserAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User Created Successfully");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO model)
        {
            // Try to find user by username first
            var user = await _userService.FindByUsernameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            // Username found. Now check password
            var signInResult = await _userService.CheckSignInAsync(user, model.Password);
            if (!signInResult.Succeeded)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim("Username", user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("FullName", $"{user.Name} {user.Surname}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(jwtToken);

        }
    }
}
