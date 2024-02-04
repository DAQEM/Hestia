namespace Hestia.Application.Dtos.Project;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public long Downloads { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeId { get; set; }
    public string? CurseForgeUrl { get; set; }
    public string? ModrinthId { get; set; }
    public string? ModrinthUrl { get; set; }
    
    public static ProjectDto FromModel(Domain.Models.Project project)    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Summary = project.Summary,
            Description = project.Description,
            ImageUrl = project.ImageUrl,
            Downloads = project.Downloads,
            GitHubUrl = project.GitHubUrl,
            CurseForgeId = project.CurseForgeId,
            CurseForgeUrl = project.CurseForgeUrl,
            ModrinthId = project.ModrinthId,
            ModrinthUrl = project.ModrinthUrl
        };
    }

    public Domain.Models.Project ToModel()
    {
        return new Domain.Models.Project
        {
            Id = Id,
            Name = Name,
            Summary = Summary,
            Description = Description,
            ImageUrl = ImageUrl,
            Downloads = Downloads,
            GitHubUrl = GitHubUrl,
            CurseForgeId = CurseForgeId,
            CurseForgeUrl = CurseForgeUrl,
            ModrinthId = ModrinthId,
            ModrinthUrl = ModrinthUrl
        };
    }
}