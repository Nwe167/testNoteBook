using BCrypt.Net;
using Dapper;
using NotesApplication.API.Data;
using NotesApplication.API.Models;
using NotesApplication.API.Repositories.Interfaces;

namespace NotesApplication.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            using var connection = _context.CreateConnection();

            var exists = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email=@Email",
                new { user.Email });

            if (exists != null)
                return false;

            var sql = @"
                INSERT INTO Users
                (
                    FullName,
                    Email,
                    PasswordHash
                )
                VALUES
                (
                    @FullName,
                    @Email,
                    @PasswordHash
                )";

            await connection.ExecuteAsync(sql, new
            {
                user.FullName,
                user.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash)
            });

            return true;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            using var connection = _context.CreateConnection();

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email=@Email",
                new { Email = email });

            if (user == null || user.PasswordHash == null)
                return null;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email=@Email",
                new
                {
                    Email = email
                });
        }
        public async Task<User> UpsertGoogleUserAsync(string googleId, string email, string fullName, string? picture)
        {
            using var connection = _context.CreateConnection();

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE GoogleId=@GoogleId OR Email=@Email",
                new { GoogleId = googleId, Email = email });

            if (user == null)
            {
                var sql = @"
                    INSERT INTO Users (FullName, Email, GoogleId, ProfilePicture)
                    VALUES (@FullName, @Email, @GoogleId, @Picture);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql,
                    new { FullName = fullName, Email = email, GoogleId = googleId, Picture = picture });

                user = await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE UserId=@Id", new { Id = newId });
            }
            else if (user.GoogleId == null)
            {
                await connection.ExecuteAsync(
                    "UPDATE Users SET GoogleId=@GoogleId, ProfilePicture=@Picture WHERE UserId=@UserId",
                    new { GoogleId = googleId, Picture = picture, user.UserId });
                user.GoogleId = googleId;
            }

            return user!;
        }
    }
}