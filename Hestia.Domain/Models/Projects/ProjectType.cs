using System.Text.Json.Serialization;
using Hestia.Domain.Converters.Json;

namespace Hestia.Domain.Models.Projects;

[JsonConverter(typeof(LowercaseStringEnumConverter<ProjectType>))]
public enum ProjectType
{
    Unknown = 0,
    Modpack = 1,
    Mod = 2,
    Plugin = 3,
    ResourcePack = 4,
    DataPack = 5,
    Shader = 6,
}