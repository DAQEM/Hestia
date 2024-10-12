using System.Net;
using Hestia.Application.Models.Responses;
using Hestia.Application.Models.Responses.Auth.OAuth;
using Hestia.Application.Options;
using Hestia.Application.Result;
using Hestia.Domain.Models.Auth;
using Hestia.Domain.Result;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hestia.Application.Services.Auth;

public class OAuthProviderService(IOptions<ApplicationOptions> applicationOptions, IOptions<AuthOptions> oAuthOptions, OAuthStateService oAuthStateService)
{
    private readonly ApplicationOptions _applicationOptions = applicationOptions.Value;
    private readonly AuthOptions _authOptions = oAuthOptions.Value;

    public async Task<string> GetLoginUrl(OAuthProvider provider, string returnUrl, int? userId = null)
    {
        string state = await oAuthStateService.AddStateAsync(provider, returnUrl, userId);
        
        switch (provider)
        {
            case OAuthProvider.Discord:
            {
                AuthDiscordOptions options = _authOptions.Discord;
                return $"https://discord.com/api/oauth2/authorize?client_id={options.ClientId}&redirect_uri={_applicationOptions.HestiaUrl + options.Callback}&response_type=code&scope={options.Scopes}&state={state}";
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
        }
    }


    public async Task<IResult<string?>> GetTokenUsingCode(OAuthProvider provider, string code)
    {
        if (provider == OAuthProvider.Discord)
        {
            AuthDiscordOptions options = _authOptions.Discord;
            Dictionary<string, string> data = new()
            {
                {"client_id", options.ClientId},
                {"client_secret", options.ClientSecret},
                {"grant_type", "authorization_code"},
                {"code", code},
                {"redirect_uri", _applicationOptions.HestiaUrl + options.Callback},
                {"scope", options.Scopes}
            };
            
            FormUrlEncodedContent content = new(data);
            using HttpClient client = new();
            HttpResponseMessage response = await client.PostAsync("https://discord.com/api/oauth2/token", content);
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new ServiceResult<string?>
                {
                    Data = null,
                    Success = false,
                    Message = "Invalid code"
                };
            }
            
            string responseContent = await response.Content.ReadAsStringAsync();
            dynamic? json = JsonConvert.DeserializeObject(responseContent);
            return new ServiceResult<string?>
            {
                Data = json?.access_token!,
                Success = true,
                Message = "Token retrieved"
            };
        }

        return new ServiceResult<string?>
        {
            Data = null,
            Success = false,
            Message = "Invalid provider"
        };
    }

    public async Task<IResult<OAuthUserResponse?>> GetUser(OAuthProvider provider, string token)
    {
        OAuthUserResponse? oAuthUser = null;
        
        if (provider == OAuthProvider.Discord)
        {
            HttpResponseMessage response;
            
            using (HttpClient client = new()) {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                response = await client.GetAsync("https://discord.com/api/users/@me");
            }
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new ServiceResult<OAuthUserResponse?>
                {
                    Data = null,
                    Success = false,
                    Message = "Invalid token"
                };
            }
            
            string responseContent = await response.Content.ReadAsStringAsync();
            dynamic? json = JsonConvert.DeserializeObject(responseContent);

            if (json is null)
            {
                return new ServiceResult<OAuthUserResponse?>
                {
                    Data = null,
                    Success = false,
                    Message = "Invalid token"
                };
            }
            
            bool isVerified = json.verified!;
            
            oAuthUser = new OAuthUserResponse
            {
                Id = json.id!,
                Username = json.username!,
                Email = isVerified ? json.email! : null,
                ImageUrl = json.avatar is not null ? $"https://cdn.discordapp.com/avatars/{json.id}/{json.avatar}.webp" : null
            };
            
            return new ServiceResult<OAuthUserResponse?>
            {
                Data = oAuthUser,
                Success = true,
                Message = "User retrieved"
            };
        }

        if (oAuthUser is null)
        {
            return new ServiceResult<OAuthUserResponse?>
            {
                Data = null,
                Success = false,
                Message = "Invalid provider"
            };
        }
        
        return new ServiceResult<OAuthUserResponse?>
        {
            Data = oAuthUser,
            Success = true,
            Message = "User retrieved"
        };
    }
}