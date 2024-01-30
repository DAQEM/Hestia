using System.Security.Claims;
using Hestia.Application.Dtos.User;
using Hestia.Application.Extensions;
using Hestia.Application.Result;
using Hestia.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers.V1;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/[controller]")]
public class UserController(ILogger<HestiaController> logger, UserService userService) : HestiaController(logger)
{
    [HttpGet("{name}")]
    public async Task<IActionResult> GetUserByName(string name)
    {
        IResult<UserDto?> result = await userService.GetUserByUserNameAsync(name);
        
        if (result.Data is  not null)
        {
            UserDto user = result.Data;
            Claim? emailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim is null || !emailClaim.Value.EqualsIgnoreCase(user.Email))
            {
                user.Email = null!;
            }
        }
        
        return HandleResult(result);
    }
    
}