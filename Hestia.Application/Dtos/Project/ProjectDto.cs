using Hestia.Application.Dtos.User;

namespace Hestia.Application.Dtos.Project;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string? BannerUrl { get; set; }
    public long Downloads { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeId { get; set; }
    public string? CurseForgeUrl { get; set; }
    public string? ModrinthId { get; set; }
    public string? ModrinthUrl { get; set; }
    public string Type { get; set; } = null!;
    public string[] Categories { get; set; } = [];
    public string[] Loaders { get; set; } = [];
    public List<UserDto> Users { get; set; } = [];
    
    public static ProjectDto FromModel(Domain.Models.Project project)    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Summary = project.Summary,
            Description = project.Description,
            Slug = project.Slug,
            ImageUrl = project.ImageUrl,
            BannerUrl = project.BannerUrl,
            Downloads = project.Downloads,
            GitHubUrl = project.GitHubUrl,
            CurseForgeId = project.CurseForgeId,
            CurseForgeUrl = project.CurseForgeUrl,
            ModrinthId = project.ModrinthId,
            ModrinthUrl = project.ModrinthUrl,
            Type = project.Type.ToString().ToLower(),
            Categories = project.Categories.Select(c => c.Name).ToArray(),
            Loaders = project.Loaders == 0 ? [] : project.Loaders.ToString().Split(", "),
            Users = project.Users.Select(UserDto.FromModel).ToList()
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
            Slug = Slug,
            ImageUrl = ImageUrl,
            BannerUrl = BannerUrl,
            Downloads = Downloads,
            GitHubUrl = GitHubUrl,
            CurseForgeId = CurseForgeId,
            CurseForgeUrl = CurseForgeUrl,
            ModrinthId = ModrinthId,
            ModrinthUrl = ModrinthUrl
        };
    }
}