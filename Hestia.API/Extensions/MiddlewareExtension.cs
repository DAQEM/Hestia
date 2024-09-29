using Hestia.API.Middleware;

namespace Hestia.API.Extensions;

public static class MiddlewareExtension
{
    public static void UseHestiaMiddleware(this IApplicationBuilder builder)
    {
        // builder.UseMiddleware<HestiaMiddleware>();
        // builder.UseMiddleware<DatabaseExceptionMiddleware>();
    }
}