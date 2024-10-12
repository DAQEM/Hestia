using AutoMapper;
using Hestia.Application.Dtos.Auth;
using Hestia.Application.Models.Responses.Auth.Sessions;
using Hestia.Application.Services.Auth;
using Hestia.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hestia.API.Controllers.V1.Auth;

[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/auth/sessions")]
public class SessionController(ILogger<HestiaController> logger, SessionService sessionService, IMapper mapper)
    : HestiaController(logger)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Get all the user's sessions",
        Description = "Get all the user's sessions",
        OperationId = "GetSessions",
        Tags = ["Auth"]
    )]
    public async Task<IActionResult> GetSessions()
    {
        string? userId = User.GetId();

        if (userId is null || !int.TryParse(userId, out int id))
        {
            return Unauthorized();
        }
    
        string? token = User.GetToken();
        
        List<SessionDto> sessions = await sessionService.GetAllByUserAsync(id);
        return Ok(sessions.Select(session => mapper.Map<SessionResponse>(session, options =>
        {
            options.AfterMap((src, dest) =>
            {
                dest.IsCurrentSession = session.Token == token;
                dest.ExpiresAt = session.RefreshExpiresAt;
            });
        })).ToList());
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Delete a session",
        Description = "Delete a session",
        OperationId = "DeleteSession",
        Tags = ["Auth"]
    )]
    public async Task<IActionResult> DeleteSession([FromRoute] int id)
    {
        string? userId = User.GetId();
        
        if (userId is null || !int.TryParse(userId, out int userIdInt))
        {
            return Unauthorized();
        }
        
        SessionDto? session = await sessionService.GetByIdAsync(userIdInt, id);
        
        if (session is null)
        {
            return Unauthorized();
        }
        
        await sessionService.DeleteByIdAsync(id);
        return NoContent();
    }
}