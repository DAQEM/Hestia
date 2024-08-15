using System.Text.Json.Serialization;

namespace Hestia.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum ProjectOrder
{
    Relevance,
    Name,
    Downloads,
    CreatedAt
}