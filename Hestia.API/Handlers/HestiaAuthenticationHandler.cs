using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

namespace Hestia.API.Handlers;

public class HestiaAuthenticationHandler : CookieAuthenticationHandler
{
    public HestiaAuthenticationHandler(IOptionsMonitor<CookieAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }
    
    private const string SessionIdClaim = "Microsoft.AspNetCore.Authentication.Cookies-SessionId";
    
    private string? _sessionKey;
    private Task<AuthenticateResult>? _readCookieTask;

    protected new CookieAuthenticationEvents Events
    {
        get => base.Events;
        set => base.Events = value;
    }

    protected override Task InitializeHandlerAsync()
    {
        Context.Response.OnStarting(FinishResponseAsync);
        return Task.CompletedTask;
    }

    protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new CookieAuthenticationEvents());

    private Task<AuthenticateResult> EnsureCookieTicket()
    {
        return _readCookieTask ??= ReadCookieAndBearerTicket();
    }

    private async Task CheckForRefreshAsync(AuthenticationTicket ticket)
    {
        DateTimeOffset currentUtc = TimeProvider.GetUtcNow();
        DateTimeOffset? issuedUtc = ticket.Properties.IssuedUtc;
        DateTimeOffset? expiresUtc = ticket.Properties.ExpiresUtc;
        bool allowRefresh = ticket.Properties.AllowRefresh ?? true;
        if (issuedUtc != null && expiresUtc != null && Options.SlidingExpiration && allowRefresh)
        {
            TimeSpan timeElapsed = currentUtc.Subtract(issuedUtc.Value);
            TimeSpan timeRemaining = expiresUtc.Value.Subtract(currentUtc);

            CookieSlidingExpirationContext eventContext = new(Context, Scheme, Options, ticket, timeElapsed, timeRemaining)
            {
                ShouldRenew = timeRemaining < timeElapsed,
            };
            await Events.CheckSlidingExpiration(eventContext);
        }
    }

    private async Task<AuthenticateResult> ReadCookieAndBearerTicket()
    {
        string? cookie = Options.CookieManager.GetRequestCookie(Context, Options.Cookie.Name!);
        if (string.IsNullOrEmpty(cookie))
        {
            string? bearer = Context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").LastOrDefault();
            if (string.IsNullOrEmpty(bearer))
            {
                return AuthenticateResult.NoResult();
            }
            cookie = bearer;
        }

        AuthenticationTicket? ticket = Options.TicketDataFormat.Unprotect(cookie, GetTlsTokenBinding());
        if (ticket == null)
        {
            return AuthenticateResults.FailedUnprotectingTicket;
        }

        if (Options.SessionStore != null)
        {
            Claim? claim = ticket.Principal.Claims.FirstOrDefault(c => c.Type.Equals(SessionIdClaim));
            if (claim == null)
            {
                return AuthenticateResults.MissingSessionId;
            }
            // Only store _sessionKey if it matches an existing session. Otherwise we'll create a new one.
            ticket = await Options.SessionStore.RetrieveAsync(claim.Value, Context, Context.RequestAborted);
            if (ticket == null)
            {
                return AuthenticateResults.MissingIdentityInSession;
            }
            _sessionKey = claim.Value;
        }

        DateTimeOffset currentUtc = TimeProvider.GetUtcNow();
        DateTimeOffset? expiresUtc = ticket.Properties.ExpiresUtc;

        if (expiresUtc != null && expiresUtc.Value < currentUtc)
        {
            if (Options.SessionStore != null)
            {
                await Options.SessionStore.RemoveAsync(_sessionKey!, Context, Context.RequestAborted);

                // Clear out the session key if its expired, so renew doesn't try to use it
                _sessionKey = null;
            }
            return AuthenticateResults.ExpiredTicket;
        }

        // Finally we have a valid ticket
        return AuthenticateResult.Success(ticket);
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult result = await EnsureCookieTicket();
        if (!result.Succeeded)
        {
            return result;
        }

        // We check this before the ValidatePrincipal event because we want to make sure we capture a clean clone
        // without picking up any per-request modifications to the principal.
        await CheckForRefreshAsync(result.Ticket);

        Debug.Assert(result.Ticket != null);
        CookieValidatePrincipalContext context = new(Context, Scheme, Options, result.Ticket);
        await Events.ValidatePrincipal(context);

        if (context.Principal == null)
        {
            return AuthenticateResults.NoPrincipal;
        }

        return AuthenticateResult.Success(new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name));
    }

    private string? GetTlsTokenBinding()
    {
        byte[]? binding = Context.Features.Get<ITlsTokenBindingFeature>()?.GetProvidedTokenBindingId();
        return binding == null ? null : Convert.ToBase64String(binding);
    }

    private static class AuthenticateResults
    {
        internal static AuthenticateResult FailedUnprotectingTicket = AuthenticateResult.Fail("Unprotect ticket failed");
        internal static AuthenticateResult MissingSessionId = AuthenticateResult.Fail("SessionId missing");
        internal static AuthenticateResult MissingIdentityInSession = AuthenticateResult.Fail("Identity missing in session store");
        internal static AuthenticateResult ExpiredTicket = AuthenticateResult.Fail("Ticket expired");
        internal static AuthenticateResult NoPrincipal = AuthenticateResult.Fail("No principal.");
    }
}