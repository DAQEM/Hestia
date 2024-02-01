namespace Hestia.Domain.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string? str1, string? str2)
    {
        return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }
    
    public static bool ContainsIgnoreCase(this string? str1, string? str2)
    {
        return str2 != null && str1?.IndexOf(str2, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}