using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;

namespace eShop.Web.Models
{
    public class ProductCatalogViewModel
    {
        public PagedList<ProductDto>? Products { get; set; }
        public ProductFilterViewModel? ProductFilterViewModel { get; set; }
    }
}