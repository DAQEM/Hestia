namespace Hestia.Application.Result;

public interface IResult<out T>
{
    T? Data { get; }
    bool Success { get; }
    string? Message { get; set; }
}