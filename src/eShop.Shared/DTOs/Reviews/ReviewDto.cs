using System;
using System.ComponentModel.DataAnnotations;

namespace eShop.Shared.DTOs.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Rating { get; set; }
        public string Title { get; set; }
        public string Body { get; set; } = string.Empty;
        public string Reviewer { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateReviewDto
    {
        [Required(ErrorMessage = "Rating is required")]
        [Range(0.0, 5.0, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal Rating { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [StringLength(2000, ErrorMessage = "Review body cannot exceed 2000 characters")]
        public string Body { get; set; } = string.Empty;
    }
}