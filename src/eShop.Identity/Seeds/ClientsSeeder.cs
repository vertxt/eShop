using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace eShop.Identity.Seeds
{
    public static class ClientSeeder
    {
        public async static Task Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // Customer Web App
            if (await manager.FindByClientIdAsync("customer_site") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "customer_site",
                    ClientSecret = "customer_secret",
                    DisplayName = "Customer Site",

                    Permissions =
                    {
                        // Grant types
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,

                        // Response types
                        Permissions.ResponseTypes.Code,

                        // Scopes
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,

                        // Endpoints
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                    },

                    RedirectUris = { new Uri("https://localhost:5001/signin-oidc") },
                    PostLogoutRedirectUris = { new Uri("https://localhost:5001/signout-callback-oidc") },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange }
                });
            }

            // Admin SPA
            if (await manager.FindByClientIdAsync("admin_site") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "admin_site",
                    ClientSecret = "admin_secret",
                    DisplayName = "Admin Site",

                    Permissions =
                    {
                        // Endpoint permissions
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,

                        // Grant type permissions
                        Permissions.GrantTypes.AuthorizationCode,

                        // Scope permissions
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,

                        // Response type permissions
                        Permissions.ResponseTypes.Code
                    },

                    RedirectUris = { new Uri("https://localhost:5002/signin-callback") },
                    PostLogoutRedirectUris = { new Uri("https://localhost:5002/signout-callback") },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange }
                });
            }
        }
    }
}