using System.Text.Json.Serialization;
using Hestia.Domain.Converters.Json;

namespace Hestia.Domain.Models.Auth;

[Flags]
[JsonConverter(typeof(LowercaseStringFlagsEnumConverter<Roles>))]
public enum Roles
{
    None = 0,
    Player = 1 << 0,
    Creator = 1 << 5,
    Moderator = 1 << 10,
    Administrator = 1 << 15
}