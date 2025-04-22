using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace eShop.Identity
{
    public static class ClientSeeder
    {
        public async static Task Seed (IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // Customer Web App
            if (await manager.FindByClientIdAsync("eShop.Web") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "eShop.Web",
                    ClientSecret = "web-client-secret",
                    DisplayName = "eShop Customer Website",
                    RedirectUris = { new Uri("https://localhost:5001/signin-oidc") },
                    PostLogoutRedirectUris = { new Uri("https://localhost:5001/signout-callback-oidc") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.EndSession,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "api"
                    }
                });
            }

            // Admin SPA
            if (await manager.FindByClientIdAsync("eShop.Admin") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "eShop.Admin",
                    ClientSecret = "admin-client-secret",
                    DisplayName = "eShop Admin Portal",
                    RedirectUris = { new Uri("https://localhost:5002/authentication/login-callback") },
                    PostLogoutRedirectUris = { new Uri("https://localhost:5002") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.EndSession,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "api"
                    }
                });
            }

            // API Resource
            var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
            if (await scopeManager.FindByNameAsync("api") == null)
            {
                await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "api",
                    DisplayName = "eShop API",
                    Resources = { "eShop.API" }
                });
            }
        }
    }
}