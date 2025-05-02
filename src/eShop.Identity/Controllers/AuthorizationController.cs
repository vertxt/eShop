using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore.Identity;
using eShop.Data.Entities.UserAggregate;
using System.Security.Claims;

namespace eShop.Identity.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthorizationController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("~/connect/authorize")]
        public async Task<IActionResult> Authorize()
        {
            // Extract OIDC request
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (!User.Identity.IsAuthenticated)
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                        Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                };

                return Challenge(properties, IdentityConstants.ApplicationScheme);
            }

            // Retrieve the user from the database
            var user = await _userManager.GetUserAsync(User) ??
                throw new InvalidOperationException("The user details cannot be retrieved.");

            // Create a new ClaimsPrincipal with the required claims
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Ensure a subject claim exists
            if (principal.FindFirst(Claims.Subject) == null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                principal.SetClaim(Claims.Subject, userId);
            }

            // Add role claims if the user has any roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                principal.SetClaim(Claims.Role, role);
            }

            // Set the requested scopes as principal scopes
            principal.SetScopes(request.GetScopes());

            // Set appropriate destinations for each claim
            principal.SetScopes(request.GetScopes());
            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            // Sign in the user
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpPost("~/connect/token")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
            {
                // Authenticate the request to validate the authorization code or refresh token
                var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                if (result.Succeeded)
                {
                    var principal = result.Principal;
                    // Optionally, you can modify the principal here (e.g., add additional claims)
                    return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }
                else
                {
                    // Return a forbidden response if the token is invalid or expired
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                        }));
                }
            }

            // Handle unsupported grant types
            return BadRequest(new OpenIddictResponse
            {
                Error = Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }

        [HttpGet("~/connect/logout")]
        public async Task<IActionResult> Logout()
        {
            // Extract the OpenIddict request
            var request = HttpContext.GetOpenIddictServerRequest();
            
            // Sign out the user from Identity
            await _signInManager.SignOutAsync();
            
            // Sign out from the authentication cookie
            await HttpContext.SignOutAsync();
            
            // Create properties for redirect
            var properties = new AuthenticationProperties();
            
            // If redirect URI was provided, use it
            if (!string.IsNullOrEmpty(request?.PostLogoutRedirectUri))
            {
                properties.RedirectUri = request.PostLogoutRedirectUri;
                
                // Forward state parameter if provided
                if (!string.IsNullOrEmpty(request.State))
                {
                    properties.Items["state"] = request.State;
                }
            }
            
            // Sign out from OpenIddict
            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: properties);
        }

        // Helper to decide which claim goes into access_token vs id_token
        static IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            switch (claim.Type)
            {
                case Claims.Name:
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;
                    yield break;

                case Claims.Email:
                    yield return Destinations.AccessToken;
                    if (principal.HasScope(Scopes.Email))
                        yield return Destinations.IdentityToken;
                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;
                    if (principal.HasScope(Scopes.Roles))
                        yield return Destinations.IdentityToken;
                    yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }
    }
}