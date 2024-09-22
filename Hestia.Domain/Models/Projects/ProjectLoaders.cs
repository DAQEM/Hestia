using System.Text.Json.Serialization;
using Hestia.Domain.Converters.Json;

namespace Hestia.Domain.Models.Projects;

[Flags]
[JsonConverter(typeof(LowercaseStringEnumConverter<ProjectLoaders>))]
public enum ProjectLoaders
{
    None = 0,
    Forge = 1,
    Fabric = 2,
    NeoForge = 4,
    Quilt = 8
}