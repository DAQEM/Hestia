namespace Hestia.Infrastructure.Exceptions;

public class ProjectNotFoundException(int id)
    : NotFoundException($"Project with id {id} was not found.")
{
    
}