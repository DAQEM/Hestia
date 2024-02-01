using Hestia.API.Middleware;

namespace Hestia.API.Extensions;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseHestiaMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HestiaMiddleware>();
    }
}