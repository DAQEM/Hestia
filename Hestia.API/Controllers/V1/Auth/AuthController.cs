using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using Hestia.Application.Dtos.Users;
using Hestia.Application.Models.Responses;
using Hestia.Application.Options;
using Hestia.Application.Services;
using Hestia.Application.Services.Auth;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Hestia.API.Controllers.V1.Auth;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/auth")]
public class AuthController(
    ILogger<HestiaController> logger,
    OAuthProviderService oAuthProviderService,
    OAuthStateService oAuthStateService,
    UserService userService,
    SessionService sessionService,
    IOptions<AuthOptions> authOptions,
    IOptions<ApplicationOptions> applicationOptions)
    : HestiaController(logger)
{
    private readonly AuthOptions _authOptions = authOptions.Value;
    private readonly ApplicationOptions _applicationOptions = applicationOptions.Value;

    [HttpGet("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [SwaggerOperation(
        Summary = "Redirects the user to the OAuth provider's login page",
        Description = "Redirects the user to the OAuth provider's login page",
        OperationId = "Login",
        Tags = ["Auth"]
    )]
    public async Task<IActionResult> Login([FromQuery, Required] OAuthProvider provider,
        [FromQuery, Required] string returnUrl)
    {
        if (!Regex.IsMatch(returnUrl, _applicationOptions.OAuthRedirectUrlRegex))
        {
            return BadRequest("Invalid return url");
        }

        string redirectUrl = await oAuthProviderService.GetLoginUrl(provider, returnUrl);

        return Redirect(redirectUrl);
    }

    [HttpGet("callback")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [SwaggerOperation(
        Summary = "Callback endpoint for OAuth providers",
        Description = "Callback endpoint for OAuth providers",
        OperationId = "Callback",
        Tags = ["Auth"]
    )]
    public async Task<IActionResult> Callback([FromQuery, Required] string code, [FromQuery, Required] string state)
    {
        OAuthState? oAuthState = await oAuthStateService.GetStateAsync(state);

        if (oAuthState is null)
        {
            return BadRequest("Invalid state");
        }

        if (oAuthState.ExpiresAt < DateTime.UtcNow)
        {
            return BadRequest("State expired");
        }

        if (!Regex.IsMatch(oAuthState.ReturnUri, _applicationOptions.OAuthRedirectUrlRegex))
        {
            return BadRequest("Invalid return url");
        }
        
        IResult<string?> tokenResult = await oAuthProviderService.GetTokenUsingCode(oAuthState.Provider, code);

        if (tokenResult.Failed || tokenResult.Data is null)
        {
            return BadRequest("Invalid code");
        }

        IResult<OAuthUserResponse?> oAuthUserIdResult = await oAuthProviderService.GetUser(oAuthState.Provider, tokenResult.Data);
        OAuthUserResponse? oAuthUser = oAuthUserIdResult.Data;
        
        if (oAuthUserIdResult.Failed || oAuthUser is null)
        {
            return BadRequest("Invalid token");
        }

        if (oAuthUser.Email is null)
        {
            return BadRequest("Email must be verified");
        }

        IResult<UserDto?> existingUserResult = await userService.GetByOAuthIdAsync(oAuthState.Provider, oAuthUser.Id);
        UserDto user = existingUserResult.Data!;
        
        if (oAuthState.UserId is null)
        {
            if (existingUserResult.Failed)
            {
                // User is registering
                IResult<UserDto> userResult = await userService.AddAsync(new UserDto
                {
                    Name = oAuthUser.Username,
                    Email = oAuthUser.Email,
                    Image = oAuthUser.ImageUrl,
                    Role = Role.Player,
                    DiscordId = oAuthState.Provider == OAuthProvider.Discord ? long.Parse(oAuthUser.Id) : null
                });
                
                if (userResult.Failed || userResult.Data is null)
                {
                    return BadRequest("Failed to register user");
                }
                
                user = userResult.Data;
            }
            
            string? userAgent = HttpContext.Request.Headers.UserAgent;
            
            if (userAgent is null)
            {
                return BadRequest("User agent required");
            }
            
            IPAddress? ipAddress = HttpContext.Connection.RemoteIpAddress;
            
            if (ipAddress is null)
            {
                return BadRequest("IP address required");
            }
            
            SessionDto session = new()
            {
                Token = "ses_" + TokenService.GenerateToken(60),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                LastUsedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                RefreshExpiresAt = DateTime.UtcNow.AddDays(90),
                UserAgent = userAgent,
                IpAddress = ipAddress.ToString()
            };
            
            await sessionService.AddAsync(session);
            
            //set cookie
            Response.Cookies.Append(_authOptions.Cookie.Name, session.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = session.RefreshExpiresAt,
                Domain = _authOptions.Cookie.Domain
            });
            
            //TODO send mail to user to let them know they registered successfully
            
            return Redirect(oAuthState.ReturnUri);
        }
        else
        {
            // User is linking an account
            if (existingUserResult.Success)
            {
                return BadRequest("Account already linked");
            }
            
            await userService.UpdateOAuthIdAsync(oAuthState.UserId.Value, oAuthState.Provider, oAuthUser.Id);
            
            //TODO send mail to user to let them know their account was linked successfully
        }

        return Redirect(oAuthState.ReturnUri);
    }
}