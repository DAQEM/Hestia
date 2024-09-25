using Hestia.Application.Services;
using Hestia.Application.Services.Users;
using Hestia.Infrastructure.Extensions;

namespace Hestia.API.Middleware;

public class HestiaMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            UserService userService = context.RequestServices.GetRequiredService<UserService>();
            if (int.TryParse(context.User.GetId(), out int userId))
            {
                await userService.UpdateUserLastActive(userId, DateTime.UtcNow);
            }
        }
        await next(context);
    }
}