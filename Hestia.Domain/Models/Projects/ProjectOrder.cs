using System.Text.Json.Serialization;
using Hestia.Domain.Converters.Json;

namespace Hestia.Domain.Models.Projects;

[JsonConverter(typeof(LowercaseStringEnumConverter<ProjectOrder>))]
public enum ProjectOrder
{
    Relevance,
    Name,
    Downloads,
    CreatedAt
}