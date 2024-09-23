using Hestia.API.Models.Responses.Auth;
using Hestia.Application.Dtos.Users;
using Hestia.Application.Services;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hestia.API.Controllers.V1.User;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/users")]
public class UserController(ILogger<HestiaController> logger, UserService userService) : HestiaController(logger)
{
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UsersMeResponse), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Get the current user",
        Description = "Get the current user",
        OperationId = "GetMe",
        Tags = ["User"]
    )]
    public IActionResult GetMe()
    {
        return Ok(new UsersMeResponse
        {
            Id = int.Parse(User.GetId()!),
            Name = User.GetName()!,
            Bio = User.GetBio()!,
            Email = User.GetEmail()!,
            Image = User.GetImage(),
            Role = User.GetRole()!
        });
    }
    
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