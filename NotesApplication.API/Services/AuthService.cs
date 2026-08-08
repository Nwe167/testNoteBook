using NotesApplication.API.Models;
using NotesApplication.API.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace NotesApplication.API.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }


        // Register
        public async Task<bool> RegisterAsync(User user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash);

            return await _userRepository.RegisterAsync(user);
        }


        // Login
        public async Task<string?> LoginAsync(
            string email,
            string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;


            bool validPassword =
                VerifyPassword(password, user.PasswordHash);


            if (!validPassword)
                return null;


            return _jwtService.GenerateToken(user);
        }



        // Hash Password
        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] bytes =
                Encoding.UTF8.GetBytes(password);

            byte[] hash =
                sha256.ComputeHash(bytes);


            return Convert.ToBase64String(hash);
        }



        // Verify Password
        private bool VerifyPassword(
            string password,
            string hashPassword)
        {
            string hash =
                HashPassword(password);

            return hash == hashPassword;
        }
    }
}