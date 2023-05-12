using ServiceLayer.Exceptions;
using System.Net;
using System.Text.Json;

namespace ClientLayer.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleNotFound(context, ex);
            }
            catch (Exception ex)
            {
                await HandleUnexpectedError(context, ex);
            }
        }

        public Task HandleNotFound(HttpContext context, NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

            string result = JsonSerializer.Serialize( new { error = ex.Message } );

            return context.Response.WriteAsync(result);
        }

        public Task HandleUnexpectedError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

            string result = JsonSerializer.Serialize( new { error = ex.Message } );

            return context.Response.WriteAsync(result);
        }
    }

    public static class ErrorHandlerExtension
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
