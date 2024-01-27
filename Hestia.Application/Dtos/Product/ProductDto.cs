namespace Hestia.Application.Dtos.Product;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public long Downloads { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeUrl { get; set; }
    public string? ModrinthUrl { get; set; }
    
    public static ProductDto FromModel(Domain.Models.Product product)    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Summary = product.Summary,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Downloads = product.Downloads,
            GitHubUrl = product.GitHubUrl,
            CurseForgeUrl = product.CurseForgeUrl,
            ModrinthUrl = product.ModrinthUrl
        };
    }

    public Domain.Models.Product ToModel()
    {
        return new Domain.Models.Product
        {
            Id = Id,
            Name = Name,
            Summary = Summary,
            Description = Description,
            ImageUrl = ImageUrl,
            Downloads = Downloads,
            GitHubUrl = GitHubUrl,
            CurseForgeUrl = CurseForgeUrl,
            ModrinthUrl = ModrinthUrl
        };
    }
}