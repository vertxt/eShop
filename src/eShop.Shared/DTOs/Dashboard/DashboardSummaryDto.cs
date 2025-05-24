namespace eShop.Shared.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }
        public decimal AverageRating { get; set; }
        public long TotalInventoryValue { get; set; }
        public decimal AverageProductPrice { get; set; }
        public int LowStockProductsCount { get; set; }
    }
}