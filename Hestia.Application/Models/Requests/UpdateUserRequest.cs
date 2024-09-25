namespace Hestia.Application.Models.Requests;

public class UpdateUserRequest
{
    public string Name { get; set; } = null!;
    public string Bio { get; set; } = null!;
}