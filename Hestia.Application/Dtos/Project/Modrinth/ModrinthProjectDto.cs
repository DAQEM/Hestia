using System.Text.Json.Serialization;

namespace Hestia.Application.Dtos.Project.Modrinth;

public class ModrinthProjectDto
{
    [JsonPropertyName("client_side")]
    public string ClientSide { get; set; } = null!;
    [JsonPropertyName("server_side")]
    public string ServerSide { get; set; } = null!;
    [JsonPropertyName("game_versions")]
    public string[] GameVersions { get; set; } = null!;
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("slug")]
    public string Slug { get; set; } = null!;
    [JsonPropertyName("project_type")]
    public string ProjectType { get; set; } = null!;
    [JsonPropertyName("team")]
    public string Team { get; set; } = null!;
    [JsonPropertyName("organization")]
    public string? Organization { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
    [JsonPropertyName("body")]
    public string Body { get; set; } = null!;
    [JsonPropertyName("body_url")]
    public string? BodyUrl { get; set; }
    [JsonPropertyName("published")]
    public string Published { get; set; } = null!;
    [JsonPropertyName("updated")]
    public string Updated { get; set; } = null!;
    [JsonPropertyName("approved")]
    public string Approved { get; set; } = null!;
    [JsonPropertyName("queued")]
    public string Queued { get; set; } = null!;
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;
    [JsonPropertyName("requested_status")]
    public string RequestedStatus { get; set; } = null!;
    [JsonPropertyName("moderator_message")]
    public string? ModeratorMessage { get; set; }
    [JsonPropertyName("downloads")]
    public int Downloads { get; set; }
    [JsonPropertyName("followers")]
    public int Followers { get; set; }
    [JsonPropertyName("categories")]
    public string[] Categories { get; set; } = null!;
    [JsonPropertyName("additional_categories")]
    public string[] AdditionalCategories { get; set; } = null!;
    [JsonPropertyName("loaders")]
    public string[] Loaders { get; set; } = null!;
    [JsonPropertyName("versions")]
    public string[] Versions { get; set; } = null!;
    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; } = null!;
    [JsonPropertyName("issues_url")]
    public string IssuesUrl { get; set; } = null!;
    [JsonPropertyName("source_url")]
    public string SourceUrl { get; set; } = null!;
    [JsonPropertyName("wiki_url")]
    public string WikiUrl { get; set; } = null!;
    [JsonPropertyName("discord_url")]
    public string DiscordUrl { get; set; } = null!;
    [JsonPropertyName("color")]
    public int Color { get; set; }
}