using System.Text.Json.Serialization;
using Hestia.Application.Converters.Json;

namespace Hestia.Application.Dtos.Project;

[Flags]
[JsonConverter(typeof(LowercaseStringEnumConverter<ProjectLoadersDto>))]
public enum ProjectLoadersDto
{
    Forge = 1,
    Fabric = 2,
    NeoForge = 4,
    Quilt = 8
}