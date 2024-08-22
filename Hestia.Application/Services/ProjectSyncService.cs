using System.Net.Http.Headers;
using System.Net.Http.Json;
using Hestia.Application.Dtos.Project;
using Hestia.Application.Dtos.Project.Modrinth;
using Hestia.Domain.Models;

namespace Hestia.Application.Services;

public class ProjectSyncService(ProjectService projectService, HttpClient httpClient)
{
    public async Task<bool> SyncModrinthProject(string projectId, string modrinthApiKey)
    {
        string requestUrl = $"https://api.modrinth.com/v2/project/{projectId}";
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(modrinthApiKey);
        requestMessage.Headers.TryAddWithoutValidation("User-Agent", "DAQEM/Hestia/1.0 (admin@daqem.com)");

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode) return false;

        ModrinthProjectDto? modrinthProject;

        try
        {
            modrinthProject = await response.Content.ReadFromJsonAsync<ModrinthProjectDto>();
        }
        catch (Exception e)
        {
            return false;
        }

        if (modrinthProject == null) return false;

        ProjectDto project = new()
        {
            Name = modrinthProject.Title,
            Summary = modrinthProject.Description,
            Description = modrinthProject.Body,
            Slug = modrinthProject.Slug,
            ImageUrl = modrinthProject.IconUrl,
            BannerUrl = modrinthProject.IconUrl,
            Downloads = modrinthProject.Downloads,
            GitHubUrl = modrinthProject.SourceUrl,
            ModrinthId = modrinthProject.Id,
            ModrinthUrl = "https://modrinth.com/" + modrinthProject.ProjectType + "/" + modrinthProject.Slug,
            Loaders = modrinthProject.Loaders,
            Type = modrinthProject.ProjectType
        };

        await projectService.AddAsync(project);
        
        return true;
    }
}