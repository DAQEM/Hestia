namespace Hestia.Domain.Exceptions;

public abstract class HestiaException : Exception
{
    protected HestiaException()
    {
    }

    protected HestiaException(string? message) : base(message)
    {
    }

    protected HestiaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}