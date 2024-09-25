using System.Net.Http.Headers;
using System.Net.Http.Json;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Dtos.Projects.External;

namespace Hestia.Application.Services.Projects;

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
            GitHubUrl = modrinthProject.SourceUrl,
            ModrinthId = modrinthProject.Id,
            ModrinthUrl = "https://modrinth.com/" + modrinthProject.ProjectType + "/" + modrinthProject.Slug,
            ModrinthDownloads = modrinthProject.Downloads,
            Loaders = modrinthProject.Loaders,
            SyncedAt = DateTime.Now,
            Type = modrinthProject.ProjectType
        };

        ProjectDto? savedProject = (await projectService.GetByModrinthIdAsync(modrinthProject.Id)).Data;
        
        if (savedProject != null)
        {
            project.Id = savedProject.Id;
            project.CreatedAt = savedProject.CreatedAt;
            project.IsFeatured = savedProject.IsFeatured;
            project.IsPublished = savedProject.IsPublished;
            project.ShouldSync = savedProject.ShouldSync;
            
            await projectService.UpdateAsync(project.Id, project);
        }
        else
        {
            project.CreatedAt = DateTime.Now;
            project.IsFeatured = false;
            project.IsPublished = false;
            project.ShouldSync = true;
            
            await projectService.AddAsync(project);
        }
        
        return true;
    }

    public async Task<bool> SyncCurseForgeProject(string projectId, string curseForgeApiKey)
    {
        string requestUrl = $"https://api.curseforge.com/v1/mods/{projectId}";
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        requestMessage.Headers.Add("x-api-key", curseForgeApiKey);

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        string stringResponse = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return false;

        CurseForgeProjectDto? curseForgeProject;
        
        try
        {
            curseForgeProject = (await response.Content.ReadFromJsonAsync<CurseForgeProjectResponseDto>())!.Data;
        }
        catch (Exception e)
        {
            return false;
        }
        
        ProjectDto project = new()
        {
            CurseForgeId = curseForgeProject.Id.ToString(),
            CurseForgeUrl = curseForgeProject.Links.WebsiteUrl,
            CurseForgeDownloads = curseForgeProject.DownloadCount,
            SyncedAt = DateTime.Now
        };
        
        ProjectDto? savedProject = (await projectService.GetByCurseForgeIdAsync(curseForgeProject.Id.ToString())).Data;

        if (savedProject == null) return false;
        
        savedProject.CurseForgeId = curseForgeProject.Id.ToString();
        savedProject.CurseForgeUrl = curseForgeProject.Links.WebsiteUrl;
        savedProject.CurseForgeDownloads = curseForgeProject.DownloadCount;
        savedProject.SyncedAt = DateTime.Now;
            
        await projectService.UpdateAsync(savedProject.Id, savedProject);
        
        return true;
    }
}