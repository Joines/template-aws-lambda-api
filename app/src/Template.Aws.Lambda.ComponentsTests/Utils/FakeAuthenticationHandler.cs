using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Template.Aws.Lambda.ComponentsTests.Utils
{
    internal class FakeAuthenticationHandler
        : AuthenticationHandler<TestAuthenticationOptions>
    {
        public FakeAuthenticationHandler(
            IOptionsMonitor<TestAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.TryGetValue("Authorization", out var _))
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Without authentication"));
            }

            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(Options.Identity),
                new AuthenticationProperties(),
                "Test Scheme");

            return Task.FromResult(
                AuthenticateResult.Success(authenticationTicket));
        }
    }

    internal class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } =
            new ClaimsIdentity(
                new[]
                {
                        new Claim(ClaimTypes.NameIdentifier, "test-name-identifier")
                },
                "test");
    }

    internal static class TestAuthenticationExtensions
    {
        public const string DefaultScheme = "TestScheme";

        public static AuthenticationBuilder AddTestAuth(
            this AuthenticationBuilder builder,
            Action<TestAuthenticationOptions> configureOptions)
        {
            return builder
                .AddScheme<TestAuthenticationOptions, FakeAuthenticationHandler>(DefaultScheme, configureOptions);
        }
    }
}
