namespace Hestia.Infrastructure.Exceptions;

public class ProductNotFoundException(int id)
    : NotFoundException($"Product with id {id} was not found.")
{
    
}