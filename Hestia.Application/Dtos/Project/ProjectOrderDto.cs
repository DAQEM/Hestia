using System.Text.Json.Serialization;
using Hestia.Application.Converters.Json;

namespace Hestia.Application.Dtos.Project;

[JsonConverter(typeof(LowercaseStringEnumConverter<ProjectOrderDto>))]
public enum ProjectOrderDto
{
    Relevance,
    Name,
    Downloads,
    CreatedAt
}