using NotesApplication.API.Models;

namespace NotesApplication.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> RegisterAsync(User user);

        Task<User?> LoginAsync(string email, string password);

        Task<User?> GetByEmailAsync(string email);

        Task<User> UpsertGoogleUserAsync(string googleId, string email, string fullName, string? picture);
    }
}