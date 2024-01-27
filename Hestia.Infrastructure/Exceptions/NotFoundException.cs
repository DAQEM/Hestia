using Hestia.Domain.Exceptions;

namespace Hestia.Infrastructure.Exceptions;

public abstract class NotFoundException(string message)
    : HestiaException(message)
{
    
}