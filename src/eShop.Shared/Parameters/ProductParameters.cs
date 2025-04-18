namespace eShop.Shared.Parameters
{
    public class ProductParameters : PaginationParameters
    {
        // Searching
        public string SearchTerm { get; set; }

        // Filtering
        public int? CategoryId  { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        
        // Sorting
        public string SortBy { get; set; } = "name";
    }
}