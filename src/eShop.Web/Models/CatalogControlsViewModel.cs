using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShop.Web.Models
{
    public class CatalogControlsViewModel
    {
        public List<int> PageSizeOptions { get; set; } = new() { 8, 12, 20 };
        public List<SelectListItem> SortOptions { get; set; } = new()
        {
            new SelectListItem { Value = "name", Text = "Name: A-Z" },
            new SelectListItem { Value = "name-desc", Text = "Name: Z-A" },
            new SelectListItem { Value = "price", Text = "Price: low to high" },
            new SelectListItem { Value = "price-desc", Text = "Price: high to low" },
        };
    }
}