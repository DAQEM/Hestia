using System.Security.Claims;
using Hestia.Application.Dtos.User;
using Hestia.Application.Result;
using Hestia.Application.Services;
using Hestia.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
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
        UserDto? user = result.Data;

        if (user != null && HttpContext.User.GetEmail() != user.Email)
        {
            user.Email = null!;
        }

        return HandleResult(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserUpdateRequestDto requestDto)
    {
        if (int.TryParse(User.GetId(), out int userId))
        {
            IResult<UserDto?> result = await userService.UpdateNameAndBioAsync(userId, requestDto.Name, requestDto.Bio);
            
            return HandleResult(result);
        }
        return Unauthorized();
    }
    
}