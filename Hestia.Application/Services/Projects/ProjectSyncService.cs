using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Dtos.Projects.External;
using Hestia.Application.Result;
using Hestia.Domain.Result;

namespace Hestia.Application.Services.Projects;

public class ProjectSyncService(
    ProjectService projectService,
    ProjectCategoryService projectCategoryService,
    HttpClient httpClient)
{
    public async Task<bool> SyncModrinthProject(string projectId, string modrinthApiKey, int userId)
    {
        string requestUrl = $"https://api.modrinth.com/v2/project/{projectId}";
        HttpRequestMessage requestMessage = new(HttpMethod.Get, requestUrl);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(modrinthApiKey);
        requestMessage.Headers.TryAddWithoutValidation("User-Agent", "DAQEM/Hestia/1.0 (admin@daqem.com)");

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode) return false;

        ModrinthProjectDto? modrinthProject;

        try
        {
            modrinthProject = await response.Content.ReadFromJsonAsync<ModrinthProjectDto>();
        }
        catch (Exception)
        {
            return false;
        }

        if (modrinthProject == null) return false;

        #region Sync Categories

        IResult<List<ProjectCategoryDto>> projectCategoriesResult = await projectCategoryService.GetAllAsync();

        if (projectCategoriesResult is { Success: true, Data: not null } && modrinthProject.Categories.Length > 0)
        {
            string[] projectCategories = projectCategoriesResult.Data.Select(c => c.Slug).ToArray();
            string[] newCategories = modrinthProject.Categories.Except(projectCategories).ToArray();
            
            await projectCategoryService.AddRangeAsync(
                newCategories.Select(c =>
                    new ProjectCategoryDto
                    {
                        Name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(c).Replace("-", " "),
                        Slug = c,
                        Content = "New category"
                    }
                ).ToList());
        }

        #endregion
        
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
            SyncedAt = DateTime.UtcNow,
            Type = modrinthProject.ProjectType
        };

        ProjectDto? savedProject = (await projectService.GetByModrinthIdAsync(project.ModrinthId)).Data;
        
        if (savedProject != null)
        {
            await projectService.UpdateModrinthProjectAsync(savedProject.Id, project);
        }
        else
        {
            project.CreatedAt = DateTime.UtcNow;
            project.IsFeatured = false;
            project.IsPublished = false;
            project.ShouldSync = true;

            ServiceResult<ProjectDto> projectCreateResult = await projectService.AddAsync(project);
            if (projectCreateResult is { Success: true, Data: not null })
            {
                project = projectCreateResult.Data;
                
                await projectService.AddUserToProjectAsync(project.Id, userId);
            }
        }

        await projectCategoryService.SetProjectCategoriesAsync(project.Id, modrinthProject.Categories);

        return true;
    }

    public async Task<bool> SyncCurseForgeProject(string projectId, string curseForgeApiKey)
    {
        string requestUrl = $"https://api.curseforge.com/v1/mods/{projectId}";
        HttpRequestMessage requestMessage = new(HttpMethod.Get, requestUrl);
        requestMessage.Headers.Add("x-api-key", curseForgeApiKey);

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode) return false;

        CurseForgeProjectDto? curseForgeProject;

        try
        {
            curseForgeProject = (await response.Content.ReadFromJsonAsync<CurseForgeProjectResponseDto>())!.Data;
        }
        catch (Exception)
        {
            return false;
        }

        ProjectDto? savedProject = (await projectService.GetByCurseForgeIdAsync(curseForgeProject.Id.ToString())).Data;

        if (savedProject == null) return false;

        savedProject.CurseForgeUrl = curseForgeProject.Links.WebsiteUrl;
        savedProject.CurseForgeDownloads = curseForgeProject.DownloadCount;
        savedProject.SyncedAt = DateTime.UtcNow;

        await projectService.UpdateCurseForgeProjectAsync(savedProject.Id, savedProject);

        return true;
    }
}