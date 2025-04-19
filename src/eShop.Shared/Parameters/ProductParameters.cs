using System.Collections.Generic;

namespace eShop.Shared.Parameters
{
    public class ProductParameters : PaginationParameters
    {
        // Searching
        public string SearchTerm { get; set; }

        // Filtering
        public List<int> CategoryIds { get; set; } = new List<int>();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStock { get; set; }

        // Sorting
        public string SortBy { get; set; } = "name";
    }
}