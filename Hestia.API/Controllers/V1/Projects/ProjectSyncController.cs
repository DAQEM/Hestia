using Hestia.Application.Options;
using Hestia.Application.Services.Projects;
using Hestia.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hestia.API.Controllers.V1.Projects;

[ApiController]
[Authorize(Policy = "creator")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/projects/sync")]
public class ProjectSyncController(
    ProjectSyncService projectSyncService,
    IOptions<ApiKeyOptions> apiKeySettings,
    ILogger<ProjectSyncController> logger)
    : HestiaController(logger)
{
    [HttpPost("modrinth")]
    public async Task<IActionResult> SyncModrinthProject(string projectId)
    {
        string modrinthApiKey = apiKeySettings.Value.Modrinth;
        string? userId = User.GetId();
        if (userId is null) return Unauthorized();
        bool success = await projectSyncService.SyncModrinthProject(projectId, modrinthApiKey, int.Parse(userId));
        return success ? Ok() : BadRequest();
    }

    [HttpPost("curseforge")]
    public async Task<IActionResult> SyncCurseForgeProject(string projectId)
    {
        string curseForgeApiKey = apiKeySettings.Value.CurseForge;
        bool success = await projectSyncService.SyncCurseForgeProject(projectId, curseForgeApiKey);
        return success ? Ok() : BadRequest();
    }
}