using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Products;
using eShop.Shared.Parameters;

namespace eShop.Web.Models
{
    public class ProductCatalogViewModel
    {
        public PagedList<ProductDto>? Products { get; set; }
        public ProductParameters? CurrentParams { get; set; }
        public CatalogFilterViewModel? CatalogFilterViewModel { get; set; }
        public CatalogControlsViewModel? CatalogControlsViewModel { get; set; }
    }
}