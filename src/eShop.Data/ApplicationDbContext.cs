using eShop.Data.Entities.CartAggregate;
using eShop.Data.Entities.CategoryAggregate;
using eShop.Data.Entities.ProductAggregate;
using eShop.Data.Entities.UserAggregate;
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

            builder.Entity<ProductImage>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint(
                    name: "CK_ProductImage_OneForeignKey",
                    sql: @"(
                        (ProductId IS NOT NULL AND ProductVariantId IS NULL)
                        OR (ProductId IS NULL AND ProductVariantId IS NOT NULL)
                    )"
                ));
            });
            
            builder.Entity<Cart>(entity =>
            {
                entity.HasIndex(c => c.UserId)
                 .IsUnique()
                 .HasFilter("UserId IS NOT NULL");
                 
                entity.HasIndex(c => c.SessionId)
                 .IsUnique()
                 .HasFilter("SessionId IS NOT NULL");
            });
            
            builder.Entity<Product>()
                .HasIndex(p => p.Uuid)
                .IsUnique();

            builder.Entity<ProductAttribute>()
                .HasIndex(pa => new { pa.ProductId, pa.AttributeId })
                .IsUnique();
        }
    }
}
