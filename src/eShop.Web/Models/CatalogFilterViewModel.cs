using eShop.Shared.DTOs.Categories;

namespace eShop.Web.Models
{
    public class CatalogFilterViewModel
    {
        // filter options
        public List<CategoryDto>? Categories { get; set; }
        public decimal MaxPrice { get; set; } = 10000;
        public decimal MinPrice { get; set; } = 0;
    }
}