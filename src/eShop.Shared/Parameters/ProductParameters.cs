using System;
using System.Collections.Generic;

namespace eShop.Shared.Parameters
{
    public enum StockRange
    {
        All,
        OutOfStock,
        Low,
        Medium,
        High
    }

    public class ProductParameters : PaginationParameters
    {

        // Searching
        public string SearchTerm { get; set; }

        // Sorting
        public string SortBy { get; set; } = "name";

        // Filtering
        public List<int> CategoryIds { get; set; } = new List<int>();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
        public bool? HasVariants { get; set; }
        public StockRange StockRange { get; set; } = StockRange.All;
        public DateTime? CreatedBefore { get; set; }
        public DateTime? CreatedAfter { get; set; }
    }
}
