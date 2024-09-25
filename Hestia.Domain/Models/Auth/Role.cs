using System.Text.Json.Serialization;
using Hestia.Domain.Converters.Json;

namespace Hestia.Domain.Models.Auth;

[JsonConverter(typeof(LowercaseStringEnumConverter<Role>))]
public enum Role
{
    Player = 1,
    Moderator = 10,
    Administrator = 20
}