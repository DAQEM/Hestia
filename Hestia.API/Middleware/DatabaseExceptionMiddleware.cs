using System.Net;
using System.Text.Json;

namespace Hestia.API.Middleware;

public class DatabaseExceptionMiddleware(RequestDelegate next, ILogger<DatabaseExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred while processing the request");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "An error occurred while processing the request" }));
        }
    }
}