using System;

namespace eShop.Shared.DTOs.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Rating { get; set; }
        public string Title { get; set; }
        public string Body { get; set; } = string.Empty;
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateReviewDto
    {
        public decimal Rating { get; set; }
        public string Title { get; set; }
        public string Body { get; set; } = string.Empty;
    }
}