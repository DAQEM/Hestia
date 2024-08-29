namespace Hestia.Domain.Result;

public class PagedResult<T> : IResult<T>
{
    public T? Data { get; init; }
    public bool Success { get; init; }
    public string? Message { get; set; }
    public Dictionary<string, string[]> Errors { get; set; } = new();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}