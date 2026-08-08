using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using NotesApplication.API.DTOs;
using NotesApplication.API.Models;
using NotesApplication.API.Repositories.Interfaces;
using NotesApplication.API.Services;

namespace NotesApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, JwtService jwtService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        private object BuildResponse(User user)
        {
            var token = _jwtService.GenerateToken(user);
            return new
            {
                token,
                expiresAt = DateTime.UtcNow.AddHours(2).ToString("o"),
                user = new { id = user.UserId.ToString(), email = user.Email, fullName = user.FullName }
            };
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.Password
            };

            var result = await _userRepository.RegisterAsync(user);
            if (!result)
                return BadRequest(new { message = "Email already exists." });

            var created = await _userRepository.GetByEmailAsync(dto.Email);
            return Ok(BuildResponse(created!));
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepository.LoginAsync(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(BuildResponse(user));
        }

        // POST: api/auth/google
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] GoogleAuthDto dto)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(dto.Credential, settings);

                var user = await _userRepository.UpsertGoogleUserAsync(
                    payload.Subject,
                    payload.Email,
                    payload.Name,
                    payload.Picture
                );

                return Ok(BuildResponse(user));
            }
            catch
            {
                return Unauthorized(new { message = "Invalid Google token." });
            }
        }
    }
}
