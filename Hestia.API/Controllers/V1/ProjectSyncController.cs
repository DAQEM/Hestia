using Hestia.Application.Services;
using Hestia.Infrastructure.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hestia.API.Controllers.V1;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/project/sync")]
public class ProjectSyncController(ProjectSyncService projectSyncService, IOptions<ApiKeySettings> apiKeySettings, ILogger<ProjectSyncController> logger)
    : HestiaController(logger)
{
    [HttpPost("modrinth")]
    public async Task<IActionResult> SyncModrinthProject(string projectId)
    {
        string modrinthApiKey = apiKeySettings.Value.Modrinth;
        bool success = await projectSyncService.SyncModrinthProject(projectId, modrinthApiKey);
        return success ? Ok() : BadRequest();
    }
}