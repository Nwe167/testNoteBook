using System.Net;
using System.Text.Json;

namespace NotesApplication.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;


        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }



        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    ex.Message
                );

                await HandleExceptionAsync(
                    context,
                    ex
                );
            }
        }



        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {

            context.Response.ContentType =
                "application/json";


            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;



            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = "An unexpected error occurred.",
                detail = _env.IsDevelopment() ? exception.Message : null
            };



            var json =
                JsonSerializer.Serialize(response);



            await context.Response.WriteAsync(json);
        }
    }
}