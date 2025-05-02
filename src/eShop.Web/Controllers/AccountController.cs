using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;

        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            return LocalRedirect(returnUrl);
        }

        return Challenge(
            new AuthenticationProperties { RedirectUri = returnUrl },
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }

    public IActionResult Register(string returnUrl = "/")
    {
        returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;

        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            return LocalRedirect(returnUrl);
        }

        var properties = new AuthenticationProperties { RedirectUri = returnUrl };
        properties.Items["acr_values"] = "register";

        return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Logout()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("LoggedOut", "Account"),
        };

        return SignOut(
            properties,
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }

    [HttpGet]
    public IActionResult LoggedOut()
    {
        return RedirectToAction("Index", "Home");
    }
}