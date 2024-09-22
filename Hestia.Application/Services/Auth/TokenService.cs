using System.Security.Cryptography;

namespace Hestia.Application.Services.Auth;

public class TokenService
{
    private static readonly char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    
    public static string GenerateToken(int size = 60)
    {
        char[] buffer = new char[size];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] data = new byte[size];
            rng.GetBytes(data);
            for (int i = 0; i < size; i++)
            {
                buffer[i] = Chars[data[i] % Chars.Length];
            }
        }
        return new string(buffer);
    }
}