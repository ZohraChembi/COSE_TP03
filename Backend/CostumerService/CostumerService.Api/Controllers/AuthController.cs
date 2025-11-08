using CostumerService.Api.Dtos;
using CostumerService.Api.Models;
using CostumerService.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CostumerService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;


        public AuthController(
            UserManager<User> userManager,
            IConfiguration configuration,
            IUserService userService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userService = userService;
        }

        // AuthController.cs

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateAsync(request);
            if (!result.Succeeded)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return Unauthorized("Invalid username or password.");

            //  Read JWT config safely
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Key is missing in configuration.");
            };
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            if (!double.TryParse(jwtSection["ExpirationMinutes"], out var expirationMinutes))
                expirationMinutes = 60; // default fallback

            //  Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id!),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                ExpiresAt = tokenDescriptor.Expires
            });
        }
    }
}
