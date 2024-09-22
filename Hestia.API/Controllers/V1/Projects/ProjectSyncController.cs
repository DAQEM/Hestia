using Hestia.Application.Options;
using Hestia.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hestia.API.Controllers.V1.Projects;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/projects/sync")]
public class ProjectSyncController(ProjectSyncService projectSyncService, IOptions<ApiKeyOptions> apiKeySettings, ILogger<ProjectSyncController> logger)
    : HestiaController(logger)
{
    [HttpPost("modrinth")]
    public async Task<IActionResult> SyncModrinthProject(string projectId)
    {
        string modrinthApiKey = apiKeySettings.Value.Modrinth;
        bool success = await projectSyncService.SyncModrinthProject(projectId, modrinthApiKey);
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