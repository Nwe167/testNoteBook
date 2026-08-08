using NotesApplication.API.Data;
using NotesApplication.API.Repositories.Interfaces;
using NotesApplication.API.Repositories.Implementations;
using NotesApplication.API.Services;

namespace NotesApplication.API.Extensions
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {

            // Database
            services.AddSingleton<DapperContext>();


            // Repository
            services.AddScoped<INoteRepository, NoteRepository>();

            services.AddScoped<IUserRepository, UserRepository>();


            // Services
            services.AddScoped<AuthService>();

            services.AddScoped<JwtService>();


            return services;
        }
    }
}