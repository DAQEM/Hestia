using Hestia.Domain.Models;

namespace Hestia.Application.Dtos.User;

public class AccountDto
{
    public int Id { get; set; }
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public long ExpiresAt { get; set; }
    public string Scope { get; set; } = null!;
    public string TokenType { get; set; } = null!;
    public string ProviderAccountId { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string Type { get; set; } = null!;

    public static AccountDto FromModel(Account? account)
    {
        if (account is null) return new AccountDto();
        
        return new AccountDto
        {
            Id = account.Id,
            AccessToken = account.AccessToken,
            RefreshToken = account.RefreshToken,
            ExpiresAt = account.ExpiresAt,
            Scope = account.Scope,
            TokenType = account.TokenType,
            ProviderAccountId = account.ProviderAccountId,
            Provider = account.Provider,
            Type = account.Type,
        };
    }

    public Account ToModel()
    {
        return new Account
        {
            Id = Id,
            AccessToken = AccessToken,
            RefreshToken = RefreshToken,
            ExpiresAt = ExpiresAt,
            Scope = Scope,
            TokenType = TokenType,
            ProviderAccountId = ProviderAccountId,
            Provider = Provider,
            Type = Type,
        };
    }
}