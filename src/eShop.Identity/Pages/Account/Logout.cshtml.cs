using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenIddict.Server.AspNetCore;

namespace eShop.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        [BindProperty]
        public string PostLogoutRedirectUri { get; set; }
        [BindProperty]
        public string State { get; set; }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Always clear the local cookie
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // Did OpenIddict hand us an end-session request?
            var oidcRequest = HttpContext.GetOpenIddictServerRequest();
            if (oidcRequest is null)
            {
                // Not an OIDC logout: just redirect home (or returnUrl)
                return LocalRedirect(returnUrl);
            }

            // It is an OIDC logout: flow through to the end-session endpoint
            var props = new AuthenticationProperties
            {
                RedirectUri = oidcRequest.PostLogoutRedirectUri ?? returnUrl
            };

            if (!string.IsNullOrEmpty(oidcRequest.State))
            {
                props.Items["state"] = oidcRequest.State;
            }

            // This will call /connect/logout under the covers
            return SignOut(
                props,
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            );
        }
    }
}