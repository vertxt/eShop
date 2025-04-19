using eShop.Shared.DTOs.Categories;
using eShop.Shared.Parameters;

namespace eShop.Web.Models
{
    public class ProductFilterViewModel
    {
        // filter options
        public List<CategoryDto>? Categories { get; set; }
        public decimal MaxPrice { get; set; } = 10000;
        public decimal MinPrice { get; set; } = 0;

        // chosen options
        public ProductParameters? CurrentParams { get; set; }
    }
}