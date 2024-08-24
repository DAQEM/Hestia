using System.Text.Json.Serialization;

namespace Hestia.Application.Dtos.Project.External;

public class CurseForgeProjectDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("gameId")]
    public int GameId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("slug")]
    public string Slug { get; set; } = null!;
    [JsonPropertyName("links")]
    public CurseForgeProjectLinksDto Links { get; set; } = null!;
    [JsonPropertyName("summary")]
    public string Summary { get; set; } = null!;
    [JsonPropertyName("status")]
    public int Status { get; set; }
    [JsonPropertyName("downloadCount")]
    public int DownloadCount { get; set; }
    [JsonPropertyName("isFeatured")]
    public bool IsFeatured { get; set; }
    [JsonPropertyName("primaryCategoryId")]
    public int PrimaryCategoryId { get; set; }
    [JsonPropertyName("classId")]
    public int ClassId { get; set; }
    [JsonPropertyName("mainFileId")]
    public int MainFileId { get; set; }
    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }
    [JsonPropertyName("dateModified")]
    public DateTime DateModified { get; set; }
    [JsonPropertyName("dateReleased")]
    public DateTime DateReleased { get; set; }
    [JsonPropertyName("allowModDistribution")]
    public bool AllowModDistribution { get; set; }
    [JsonPropertyName("gamePopularityRank")]
    public int GamePopularityRank { get; set; }
    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; set; }
    [JsonPropertyName("thumbsUpCount")]
    public int ThumbsUpCount { get; set; }
}