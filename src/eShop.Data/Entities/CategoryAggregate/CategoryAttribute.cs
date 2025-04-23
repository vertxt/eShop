namespace eShop.Data.Entities.CategoryAggregate
{
    public class CategoryAttribute
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public required string Name { get; set; }
        public string? DisplayName { get; set; }
    }
}