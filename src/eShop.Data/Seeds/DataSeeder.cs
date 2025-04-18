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

            var categories = new List<Category>
            {
                new Category { Name = "Electronics", Description = "Devices and gadgets" },
                new Category { Name = "Books", Description = "Printed and digital books" },
                new Category { Name = "Clothing", Description = "Apparel for men, women, and children" },
                new Category { Name = "Home & Kitchen", Description = "Appliances and utensils" },
                new Category { Name = "Sports & Outdoors", Description = "Gear and apparel for sports and outdoor activities" },
                new Category { Name = "Beauty & Personal Care", Description = "Cosmetics, skincare, and more" },
                new Category { Name = "Toys & Games", Description = "For kids and adults" },
                new Category { Name = "Automotive", Description = "Car accessories and tools" }
            };

            await context.Categories.AddRangeAsync(categories);
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
                    CategoryId = 1,
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
                    CategoryId = 1,
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
                    CategoryId = 1,
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
                    CategoryId = 1,
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
                    CategoryId = 1,
                    HasVariants = false,
                    QuantityInStock = 300,
                    CreatedDate = DateTime.UtcNow,
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Wireless Earbuds",
                    BasePrice = 59.99m,
                    Description = "High-quality wireless earbuds with noise cancellation.",
                    ShortDescription = "Wireless earbuds with ANC.",
                    IsActive = true,
                    CategoryId = 1,
                    HasVariants = true,
                    QuantityInStock = 120
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "The Pragmatic Programmer",
                    BasePrice = 39.99m,
                    Description = "Classic book on software development and best practices.",
                    ShortDescription = "A must-read for developers.",
                    IsActive = true,
                    CategoryId = 2,
                    HasVariants = false,
                    QuantityInStock = 75
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Men's Casual T-Shirt",
                    BasePrice = 15.50m,
                    Description = "Comfortable cotton T-shirt available in multiple sizes and colors.",
                    ShortDescription = "Basic cotton T-shirt.",
                    IsActive = true,
                    CategoryId = 3,
                    HasVariants = true,
                    QuantityInStock = 300
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Stainless Steel Cookware Set",
                    BasePrice = 89.99m,
                    Description = "10-piece stainless steel cookware set suitable for all stovetops.",
                    ShortDescription = "Premium cookware for your kitchen.",
                    IsActive = true,
                    CategoryId = 4,
                    HasVariants = false,
                    QuantityInStock = 45
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Yoga Mat with Carrying Strap",
                    BasePrice = 25.00m,
                    Description = "Non-slip yoga mat, ideal for all types of yoga and workouts.",
                    ShortDescription = "Durable, thick yoga mat.",
                    IsActive = true,
                    CategoryId = 5,
                    HasVariants = false,
                    QuantityInStock = 200
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Hydrating Facial Cleanser",
                    BasePrice = 12.75m,
                    Description = "Gentle facial cleanser suitable for all skin types.",
                    ShortDescription = "Daily face wash.",
                    IsActive = true,
                    CategoryId = 6,
                    HasVariants = false,
                    QuantityInStock = 180
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Remote Control Racing Car",
                    BasePrice = 34.99m,
                    Description = "High-speed remote control car with rechargeable battery.",
                    ShortDescription = "Fun toy for kids aged 6+.",
                    IsActive = true,
                    CategoryId = 7,
                    HasVariants = false,
                    QuantityInStock = 95
                },
                new Product
                {
                    Uuid = Guid.NewGuid().ToString(),
                    Name = "Car Vacuum Cleaner",
                    BasePrice = 45.00m,
                    Description = "Portable vacuum cleaner for cars with powerful suction and USB charging.",
                    ShortDescription = "Keep your car clean effortlessly.",
                    IsActive = true,
                    CategoryId = 8,
                    HasVariants = false,
                    QuantityInStock = 60
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