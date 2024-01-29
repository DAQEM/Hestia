using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Hestia.Infrastructure.Application;
using Microsoft.AspNetCore.Authentication;
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
        returnUrl ??= _applicationSettings.BaseUrl;

        return Task.FromResult<IActionResult>(Challenge(new AuthenticationProperties
        {
            RedirectUri = returnUrl
        }, DiscordAuthenticationDefaults.AuthenticationScheme));
    }
    
    [Authorize]
    [HttpGet("logout/discord")]
    public Task<IActionResult> Logout(string? returnUrl = null)
    {
        returnUrl ??= _applicationSettings.BaseUrl;

        return Task.FromResult<IActionResult>(SignOut(new AuthenticationProperties
        {
            RedirectUri = returnUrl
        }, DiscordAuthenticationDefaults.AuthenticationScheme));
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        string? id = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        string? username = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        string? email = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        string? image = User.Claims.FirstOrDefault(claim => claim.Type == "image")?.Value;

        if (id is null || username is null || image is null || email is null)
        {
            return BadRequest(new
            {
                Success = false,
                Message = "Invalid user"
            });
        }
        
        return Ok(new
        {
            Success = true,
            Message = "User found",
            Data = new
            {
                Id = id,
                Username = username,
                Email = email,
                Image = image
            }
        });
    }
    
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new
        {
            Success = true,
            Message = "You are authenticated"
        });
    }
}