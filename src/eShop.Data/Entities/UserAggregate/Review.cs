using eShop.Data.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Entities.UserAggregate
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        [Precision(2, 1)]
        public required decimal Rating { get; set; }
        public required string Title { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}