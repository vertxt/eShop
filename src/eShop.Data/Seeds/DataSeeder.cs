using eShop.Data.Entities.Categories;
using eShop.Data.Entities.Products;
using eShop.Data.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace eShop.Data.Seeds
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider;

            try
            {
                // Seed data here
                var context = service.GetRequiredService<ApplicationDbContext>();
                // var userManager = service.GetRequiredService<UserManager<User>>();
                // var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
                // var applicationManager = service.GetRequiredService<IOpenIddictApplicationManager>();

                await context.Database.EnsureCreatedAsync();

                // await SeedRolesAsync(roleManager);
                // await SeedUsersAsync(userManager);
                // await SeedClientsAsync(applicationManager);
                await SeedProductsAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            var adminUser = new User
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new InvalidOperationException($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            var customerUser = new User
            {
                UserName = "customer@example.com",
                Email = "customer@example.com",
            };

            if (await userManager.FindByEmailAsync(customerUser.Email) == null)
            {
                var result = await userManager.CreateAsync(customerUser);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer@123");
                }
                else
                {
                    throw new InvalidOperationException($"Failed to create customer user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        private static async Task SeedClientsAsync(IOpenIddictApplicationManager applicationManager)
        {
            throw new NotImplementedException("To be implemented");
        }

        private static async Task SeedProductsAsync(ApplicationDbContext context)
        {
            if (await context.Products.AnyAsync())
            {
                return;
            }

            var electronicsCategory = new Category { Name = "Electronics" };
            var accessoriesCategory = new Category { Name = "Accessories" };

            await context.Categories.AddRangeAsync(electronicsCategory, accessoriesCategory);
            var saveCategoriesResult = await context.SaveChangesAsync();

            if (saveCategoriesResult == 0)
            {
                throw new InvalidOperationException("Failed to seed categories");
            }

            ICollection<Product> products = new List<Product>
            {
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Wireless Mouse",
                    BasePrice = 25.99m,
                    Description = "A smooth and precise wireless mouse.",
                    ShortDescription = "Wireless Mouse",
                    IsActive = true,
                    CategoryId = accessoriesCategory.Id,
                    HasVariants = false,
                    QuantityInStock = 100,
                    CreatedDate = DateTime.UtcNow,
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Gaming Keyboard",
                    BasePrice = 79.99m,
                    Description = "Mechanical keyboard with RGB lighting and fast response.",
                    ShortDescription = "RGB Gaming Keyboard",
                    IsActive = true,
                    CategoryId = accessoriesCategory.Id,
                    HasVariants = true,
                    QuantityInStock = 50,
                    CreatedDate = DateTime.UtcNow,
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Smartphone",
                    BasePrice = 499.99m,
                    Description = "Mid-range smartphone with great battery life.",
                    ShortDescription = "Smartphone",
                    IsActive = true,
                    CategoryId = electronicsCategory.Id,
                    HasVariants = false,
                    QuantityInStock = 200,
                    CreatedDate = DateTime.UtcNow,
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Bluetooth Speaker",
                    BasePrice = 45.50m,
                    Description = "Portable speaker with high-quality sound.",
                    ShortDescription = "Bluetooth Speaker",
                    IsActive = true,
                    CategoryId = electronicsCategory.Id,
                    HasVariants = false,
                    QuantityInStock = 75,
                    CreatedDate = DateTime.UtcNow,
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "USB-C Charging Cable",
                    BasePrice = 9.99m,
                    Description = "Durable and fast-charging USB-C cable.",
                    ShortDescription = "USB-C Cable",
                    IsActive = true,
                    CategoryId = accessoriesCategory.Id,
                    HasVariants = false,
                    QuantityInStock = 300,
                    CreatedDate = DateTime.UtcNow,
                }
            };

            await context.Products.AddRangeAsync(products);
            var saveProductsResult = await context.SaveChangesAsync();

            if (saveProductsResult == 0)
            {
                throw new InvalidOperationException("Failed to seed products");
            }
        }
    }
}