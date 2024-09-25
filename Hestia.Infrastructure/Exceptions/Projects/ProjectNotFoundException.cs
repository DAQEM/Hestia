namespace Hestia.Infrastructure.Exceptions.Projects;

public class ProjectNotFoundException(int id)
    : NotFoundException($"Project with id {id} was not found.")
{
    
}