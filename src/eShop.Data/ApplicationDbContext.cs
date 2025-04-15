using eShop.Data.Entities.Carts;
using eShop.Data.Entities.Categories;
using eShop.Data.Entities.Products;
using eShop.Data.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
