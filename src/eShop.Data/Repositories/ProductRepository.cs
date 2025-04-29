using eShop.Data.Entities.ProductAggregate;
using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        // Query
        public IQueryable<Product> GetAllWithBasicDetails()
        {
            return _entities
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews);
        }

        public async Task<Product?> GetByUuidAsync(string uuid)
        {
            return await _entities.FirstAsync(p => p.Uuid == uuid);
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await _entities
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.Images)
                .Include(p => p.Attributes)
                    .ThenInclude(pa => pa.Attribute)
                .Include(p => p.Reviews)
                    .ThenInclude(pr => pr.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // CRUD operations
        public async Task<bool> CreateProductWithRelationsAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }

        /* UNUSED METHODS (commented out) */
        // Variant-related methods
        // public async Task<ProductVariant?> GetVariantByIdAsync(int variantId)
        // {
        //     return await _context.ProductVariants
        //         .Include(v => v.Images)
        //         .FirstOrDefaultAsync(v => v.Id == variantId);
        // }

        // public async Task<bool> AddVariantAsync(ProductVariant variant)
        // {
        //     await _context.ProductVariants.AddAsync(variant);
        //     return await _context.SaveChangesAsync() > 0;
        // }

        // public async Task<bool> UpdateVariantAsync(ProductVariant variant)
        // {
        //     _context.ProductVariants.Update(variant);
        //     return await _context.SaveChangesAsync() > 0;
        // }
        // public async Task<bool> DeleteVariantAsync(int variantId)
        // {
        //     var variant = await _context.ProductVariants.FindAsync(variantId);
        //     if (variant is null)
        //     {
        //         return false;
        //     }

        //     _context.ProductVariants.Remove(variant);
        //     return await _context.SaveChangesAsync() > 0;
        // }

        // Image-related methods
        // public async Task<ProductImage?> GetImageByIdAsync(int imageId)
        // {
        //     return await _context.ProductImages.FindAsync(imageId);
        // }
        // public IQueryable<ProductImage> GetImagesByProductId(int productId)
        // {
        //     return _context.ProductImages.Where(i => i.ProductId == productId);
        // }
        // public IQueryable<ProductImage> GetImagesByVariantId(int variantId)
        // {
        //     return _context.ProductImages.Where(i => i.ProductVariantId == variantId);
        // }
        // public async Task<bool> AddImageAsync(ProductImage image)
        // {
        //     await _context.ProductImages.AddAsync(image);
        //     return await _context.SaveChangesAsync() > 0;
        // }
        // public async Task<bool> UpdateImageAsync(ProductImage image)
        // {
        //     _context.ProductImages.Update(image);
        //     return await _context.SaveChangesAsync() > 0;
        // }
        // public async Task<bool> DeleteImageAsync(int imageId)
        // {
        //     var image = await _context.ProductImages.FindAsync(imageId);
        //     if (image is null) return false;
        //     _context.ProductImages.Remove(image);
        //     return await _context.SaveChangesAsync() > 0;
        // }
        // public async Task<bool> SetMainImageAsync(int productId, int imageId)
        // {
        //     var productImages = await _context.ProductImages
        //         .Where(i => i.ProductId == productId)
        //         .ToListAsync();

        //     foreach (var image in productImages)
        //     {
        //         image.IsMain = image.Id == imageId;
        //     }

        //     return await _context.SaveChangesAsync() > 0;
        // }
        // public async Task<bool> SetMainVariantImageAsync(int variantId, int imageId)
        // {
        //     var variantImages = await _context.ProductImages
        //         .Where(i => i.ProductVariantId == variantId)
        //         .ToListAsync();

        //     foreach (var image in variantImages)
        //     {
        //         image.IsMain = image.Id == imageId;
        //     }

        //     return await _context.SaveChangesAsync() > 0;
        // }

        // Attribute-related methods
        // public async Task<ProductAttribute?> GetAttributeByIdAsync(int attributeId)
        // {
        //     return await _context.ProductAttributes
        //         .Include(a => a.Attribute)
        //         .FirstOrDefaultAsync(a => a.Id == attributeId);
        // }

        // public async Task<bool> AddAttributeAsync(ProductAttribute attribute)
        // {
        //     await _context.ProductAttributes.AddAsync(attribute);
        //     return await _context.SaveChangesAsync() > 0;
        // }

        // public async Task<bool> UpdateAttributeAsync(ProductAttribute attribute)
        // {
        //     _context.ProductAttributes.Update(attribute);
        //     return await _context.SaveChangesAsync() > 0;
        // }

        // public async Task<bool> DeleteAttributeAsync(int attributeId)
        // {
        //     var attribute = await _context.ProductAttributes.FindAsync(attributeId);
        //     if (attribute == null) return false;

        //     _context.ProductAttributes.Remove(attribute);
        //     return await _context.SaveChangesAsync() > 0;
        // }
    }
}
