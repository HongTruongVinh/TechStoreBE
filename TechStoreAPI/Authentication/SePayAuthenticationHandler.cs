using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using TechStore.Common.Constants;

namespace TechStoreAPI.Authentication
{
    public class SePayAuthenticationHandler: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly SePaySettings _sepayConfig;

        public SePayAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IOptions<SePaySettings> sepayOptions)
            : base(options, logger, encoder)
        {
            _sepayConfig = sepayOptions.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization",out var authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header."));
            }

            var authorization = authorizationHeader.ToString();

            const string prefix = "Apikey ";

            if (!authorization.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization scheme."));
            }

            var apiKey = authorization[prefix.Length..];

            var expectedApiKey = _sepayConfig.ApiKey;

            if (string.IsNullOrWhiteSpace(expectedApiKey))
            {
                Logger.LogError("SePay API Key is not configured.");

                return Task.FromResult(AuthenticateResult.Fail("SePay authentication is not configured."));
            }

            if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(apiKey), Encoding.UTF8.GetBytes(expectedApiKey)))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key."));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "SePay"),
                new Claim("Provider", "SePay")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
