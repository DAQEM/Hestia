using System.Security.Claims;
using System.Security.Principal;
using AspNet.Security.OAuth.Discord;
using Hestia.Application.Dtos.User;
using Hestia.Application.Services;
using Hestia.Infrastructure.Application;
using Hestia.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hestia.API.Controllers.V1;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/[controller]")]
public class AuthenticationController: HestiaController
{
    private readonly ApplicationSettings _applicationSettings;

    public AuthenticationController(ILogger<HestiaController> logger, IOptions<ApplicationSettings> applicationSettings)
        : base(logger)
    {
        _applicationSettings = applicationSettings.Value;
    }

    [HttpGet("login/discord")]
    public Task<IActionResult> Login(string? returnUrl = null)
    {
        returnUrl ??= _applicationSettings.AsteriaUrl;

        return Task.FromResult<IActionResult>(Challenge(new AuthenticationProperties
        {
            RedirectUri = returnUrl,
        }, DiscordAuthenticationDefaults.AuthenticationScheme));
    }
    
    [Authorize]
    [HttpGet("logout")]
    public Task<IActionResult> Logout(string? returnUrl = null)
    {
        returnUrl ??= _applicationSettings.AsteriaUrl;

        return Task.FromResult<IActionResult>(SignOut(new AuthenticationProperties
        {
            RedirectUri = returnUrl
        }, CookieAuthenticationDefaults.AuthenticationScheme));
    }
    
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        string? id = User.GetId();
        string? name = User.GetName();
        string? email = User.GetEmail();
        string? image = User.GetImage();
        string? role = User.GetRole();

        if (id is null || name is null || email is null || role is null)
        {
            return Unauthorized();
        }

        return Ok(new UserDto
        {
            Id = int.Parse(id),
            Name = name,
            Email = email,
            Image = image,
            Role = Enum.Parse<RoleDto>(role, true)
        });
    }
    
    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        string? id = User.GetId();
        if (id is null)
        {
            return Unauthorized();
        }
        
        UserService userService = HttpContext.RequestServices.GetRequiredService<UserService>();
        UserDto? existingUser = (await userService.GetAsync(int.Parse(id))).Data;
        
        if (existingUser is null)
        {
            return Unauthorized();
        }

        IIdentity? identity = User.Identity;
        
        if (identity is not ClaimsIdentity claimsIdentity)
        {
            return Unauthorized();
        }
        
        claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Name));
        claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Email));
        claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Role));
        claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Uri));

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, existingUser.Name));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, existingUser.Email));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, existingUser.Role.ToString().ToLower()));
        
        if (existingUser.Image is not null)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Uri, existingUser.Image));
        }
        
        await HttpContext.SignOutAsync();
        await HttpContext.SignInAsync(User);

        string newToken = HttpContext.Response.Headers.SetCookie
            .ToString()
            .Split("=")
            .OrderBy(x => x.Length)
            .Last()
            .Split(";")
            .OrderBy(x => x.Length)
            .Last();

        return Ok(new { Token = newToken });
    }
}