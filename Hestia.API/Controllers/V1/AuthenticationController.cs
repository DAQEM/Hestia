using AspNet.Security.OAuth.Discord;
using Hestia.Application.Extensions;
using Hestia.Infrastructure.Application;
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
            RedirectUri = returnUrl
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
        string? username = User.GetUserName();
        string? email = User.GetEmail();
        string? image = User.GetImage();

        if (id is null || username is null || image is null || email is null)
        {
            return Unauthorized();
        }
        
        return Ok(new
        {
            Id = id,
            Username = username,
            Email = email,
            Image = image
        });
    }
}