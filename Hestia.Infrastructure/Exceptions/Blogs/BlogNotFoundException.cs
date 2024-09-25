namespace Hestia.Infrastructure.Exceptions.Blogs;

public class BlogNotFoundException(int id)
    : NotFoundException($"Blog with id {id} was not found.")
{
    
}