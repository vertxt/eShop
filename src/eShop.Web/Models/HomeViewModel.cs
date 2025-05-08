using eShop.Shared.DTOs.Categories;
using eShop.Shared.DTOs.Products;

namespace eShop.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ProductDto> FeaturedProducts { get; set; } = Enumerable.Empty<ProductDto>();
        public IEnumerable<CategoryDto> Categories { get; set; } = Enumerable.Empty<CategoryDto>();
    }
}