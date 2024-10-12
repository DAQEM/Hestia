using Hestia.Domain.Models.Users;

namespace Hestia.Infrastructure.Queries.Users;

public static class UserQueries
{
    public static List<User> SelectSimpleUsers(List<User> users)
    {
        return users.Select(u => new User
        {
            Name = u.Name,
            Bio = u.Bio,
            Image = u.Image,
            Roles = u.Roles,
            Joined = u.Joined
        }).ToList();
    }
}