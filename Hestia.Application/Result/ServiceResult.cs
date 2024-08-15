using Hestia.Domain.Result;

namespace Hestia.Application.Result;

public class ServiceResult<T> : IResult<T>
{
    public T? Data { get; init;  }
    public bool Success { get; init; }
    public string? Message { get; set; }
}