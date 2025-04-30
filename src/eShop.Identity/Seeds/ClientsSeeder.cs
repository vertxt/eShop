using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace eShop.Identity.Seeds
{
    public static class ClientSeeder
    {
        public async static Task Seed (IServiceProvider serviceProvider)
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
                    RedirectUris = { new Uri("https://localhost:5001/signin-oidc") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                    }
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
                    RedirectUris = { new Uri("https://localhost:5002/callback") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                    }
                });
            }
        }
    }
}