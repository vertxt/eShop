namespace eShop.Shared.DTOs.Dashboard
{
    public class CategoryProductCountDto
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
        public decimal PercentageOfTotal { get; set; }
    }
}