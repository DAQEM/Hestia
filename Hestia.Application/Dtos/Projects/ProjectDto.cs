using Hestia.Application.Dtos.Users;
using Hestia.Domain.Models.Projects;

namespace Hestia.Application.Dtos.Projects;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string? BannerUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeId { get; set; }
    public string? CurseForgeUrl { get; set; }
    public long CurseForgeDownloads { get; set; }
    public string? ModrinthId { get; set; }
    public string? ModrinthUrl { get; set; }
    public long ModrinthDownloads { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public bool ShouldSync { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime SyncedAt { get; set; }
    public string Type { get; set; } = null!;
    public string[] Categories { get; set; } = [];
    public string[] Loaders { get; set; } = [];
    public List<UserDto> Users { get; set; } = [];
    
    public static ProjectDto FromModel(Project project)    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Summary = project.Summary,
            Description = project.Description,
            Slug = project.Slug,
            ImageUrl = project.ImageUrl,
            BannerUrl = project.BannerUrl,
            GitHubUrl = project.GitHubUrl,
            CurseForgeId = project.CurseForgeId,
            CurseForgeUrl = project.CurseForgeUrl,
            CurseForgeDownloads = project.CurseForgeDownloads,
            ModrinthId = project.ModrinthId,
            ModrinthUrl = project.ModrinthUrl,
            ModrinthDownloads = project.ModrinthDownloads,
            IsFeatured = project.IsFeatured,
            IsPublished = project.IsPublished,
            ShouldSync = project.ShouldSync,
            CreatedAt = project.CreatedAt,
            SyncedAt = project.SyncedAt,
            Type = project.Type.ToString().ToLower(),
            Categories = project.Categories.Select(c => c.Name).ToArray(),
            Loaders = project.Loaders == 0 ? [] : project.Loaders.ToString().Split(", "),
            Users = project.Users.Select(UserDto.FromModel).ToList()
        };
    }

    public Project ToModel()
    {
        return new Project
        {
            Id = Id,
            Name = Name,
            Summary = Summary,
            Description = Description,
            Slug = Slug,
            ImageUrl = ImageUrl,
            BannerUrl = BannerUrl,
            GitHubUrl = GitHubUrl,
            CurseForgeId = CurseForgeId,
            CurseForgeUrl = CurseForgeUrl,
            CurseForgeDownloads = CurseForgeDownloads,
            ModrinthId = ModrinthId,
            ModrinthUrl = ModrinthUrl,
            ModrinthDownloads = ModrinthDownloads,
            IsFeatured = IsFeatured,
            IsPublished = IsPublished,
            ShouldSync = ShouldSync,
            CreatedAt = CreatedAt,
            SyncedAt = SyncedAt,
            Type = TryParseProjectType(Type),
            Loaders = TryParseLoaders(Loaders),
        };
    }
    
    private static ProjectType TryParseProjectType(string type)
    {
        return Enum.TryParse(type, true, out ProjectType projectType) ? projectType : ProjectType.Unknown;
    }
    
    private static ProjectLoaders TryParseLoaders(IEnumerable<string> loaders)
    {
        ProjectLoaders flags = ProjectLoaders.None;

        foreach (string inputString in loaders)
        {
            try
            {
                ProjectLoaders parsedFlag = (ProjectLoaders)Enum.Parse(typeof(ProjectLoaders), inputString, true);
                flags |= parsedFlag;
            }
            catch (ArgumentException _) {}
        }

        return flags;
    }
}