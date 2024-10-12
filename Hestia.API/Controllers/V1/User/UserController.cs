using AutoMapper;
using Hestia.Application.Dtos.Users;
using Hestia.Application.Models.Requests;
using Hestia.Application.Models.Requests.Users;
using Hestia.Application.Models.Responses.Auth;
using Hestia.Application.Models.Responses.Users;
using Hestia.Application.Services.Users;
using Hestia.Domain.Models.Auth;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hestia.API.Controllers.V1.User;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/users")]
public class UserController(ILogger<HestiaController> logger, UserService userService, IMapper mapper) : HestiaController(logger)
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
            Roles = Enum.Parse<Roles>(User.GetRole()!),
        });
    }
    
    [HttpGet("{name}")]
    public async Task<IActionResult> GetUserByName(string name)
    {
        IResult<UserDto?> result = await userService.GetUserByUserNameAsync(name);
        if (result.Data is null) return NotFound();
        if (result.Success is false) return HandleFailedResult(result);
        
        UserDto user = result.Data;
        if (int.TryParse(User.GetId(), out int userId))
        {
            if (userId == user.Id) return Ok(mapper.Map<OwnUserResponse>(user));
        }
        return Ok(mapper.Map<UserResponse>(user));
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
    {
        if (int.TryParse(User.GetId(), out int userId))
        {
            IResult<UserDto?> result = await userService.UpdateNameAndBioAsync(userId, request.Name, request.Bio);
            
            return HandleResult(result);
        }
        return Unauthorized();
    }
    
}