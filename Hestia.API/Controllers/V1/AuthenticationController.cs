using AspNet.Security.OAuth.Discord;
using Hestia.Infrastructure.Application;
using Hestia.Infrastructure.Database;
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
    private readonly HestiaDbContext _dbContext;
    private readonly ApplicationSettings _applicationSettings;

    public AuthenticationController(ILogger<HestiaController> logger, HestiaDbContext dbContext,
        IOptions<ApplicationSettings> applicationSettings)
        : base(logger)
    {
        _dbContext = dbContext;
        _applicationSettings = applicationSettings.Value;
    }

    [HttpGet("login/discord")]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        if (returnUrl == null)
        {
            returnUrl = _applicationSettings.BaseUrl;
        }
        
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = returnUrl
        }, DiscordAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet("discord/info")]
    public IActionResult Discord()
    {
        return Ok(new
        {
            Success = true,
            Message = "You are now authenticated with Discord."
        });
    }
    
    [HttpGet("login/discord/callback")]
    public async Task<IActionResult> DiscordCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            Logger.LogWarning("Authentication failed for user {User}", result.Principal);
            return Unauthorized();
        }

        return Ok();
    }
}