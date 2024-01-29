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