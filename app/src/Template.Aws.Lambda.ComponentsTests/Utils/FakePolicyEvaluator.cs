
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Template.Aws.Lambda.Application.Mappers;

namespace Template.Aws.Lambda.ComponentsTests.Utils
{
    public class FakePolicyEvaluator : IPolicyEvaluator
    {

        public virtual async Task<AuthenticateResult> AuthenticateAsync(
            AuthorizationPolicy policy, 
            HttpContext context)
        {
            var testScheme = "testScheme";
            var principal = new ClaimsPrincipal();

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Administrator")
            }, testScheme));

            return await Task.FromResult(
                AuthenticateResult.Success(
                    new AuthenticationTicket(principal,
                    new AuthenticationProperties(), testScheme)));
        }

        public virtual Task<PolicyAuthorizationResult> AuthorizeAsync(
            AuthorizationPolicy policy, 
            AuthenticateResult authenticationResult, 
            HttpContext context, 
            object? resource)
        {
            return Task.FromResult(
                PolicyAuthorizationResult.Success());
        }

        public virtual Task<PolicyAuthorizationResult> AuthorizeAsync(
            AuthorizationPolicy policy,
            AuthenticateResult authenticationResult,
            HttpContext context)
        {
            return Task.FromResult(
                PolicyAuthorizationResult.Success());
        }
    }
}
