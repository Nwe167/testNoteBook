using NotesApplication.API.Repositories.Interfaces;
using NotesApplication.API.Repositories.Implementations;
using NotesApplication.API.Data;
using NotesApplication.API.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// COOP Header Middleware (Required for Google OAuth Popups)
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Cross-Origin-Opener-Policy", "same-origin-allow-popups");
    await next();
});

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();