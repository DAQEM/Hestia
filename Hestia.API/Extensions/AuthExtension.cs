using Hestia.Domain.Models.Auth;
using Hestia.Infrastructure.Extensions;
using Hestia.Infrastructure.Handlers;
using Hestia.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;

namespace Hestia.API.Extensions;

public static class AuthExtension
{
    public static void AddHestiaAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(HestiaAuthenticationOptions.DefaultScheme)
            .AddScheme<HestiaAuthenticationOptions, HestiaAuthenticationHandler>
            (HestiaAuthenticationOptions.DefaultScheme,
                options =>
                {
                    
                });

        services.AddSingleton<IAuthorizationHandler, RoleHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("administrator", policy => policy.AddRequirements(new RoleRequirement(Roles.Administrator)));
            options.AddPolicy("moderator", policy => policy.AddRequirements(new RoleRequirement(Roles.Moderator | Roles.Administrator)));
            options.AddPolicy("creator", policy => policy.AddRequirements(new RoleRequirement(Roles.Creator | Roles.Moderator | Roles.Administrator)));
        });
    }

    public static void UseHestiaAuthentication(this IApplicationBuilder app)
    {
        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.Always
        });
        
        app.UseAuthentication();
        app.UseAuthorization();
    }
    
    private class RoleRequirement(Roles role) : IAuthorizationRequirement
    {
        public Roles Role { get; } = role;
    }
    
    private class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            string? roleClaim = context.User.GetRole();

            if (roleClaim != null && int.TryParse(roleClaim, out int roleInt))
            {
                Roles roles = (Roles)roleInt;

                if ((roles & requirement.Role) != 0)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}