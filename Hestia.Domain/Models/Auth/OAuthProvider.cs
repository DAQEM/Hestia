using System.Text.Json.Serialization;
using Hestia.Domain.Converters.Json;

namespace Hestia.Domain.Models.Auth;

[JsonConverter(typeof(LowercaseStringEnumConverter<OAuthProvider>))]
public enum OAuthProvider
{
    Discord
}