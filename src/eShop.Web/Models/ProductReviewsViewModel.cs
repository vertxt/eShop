using eShop.Shared.Common.Pagination;
using eShop.Shared.DTOs.Reviews;

namespace eShop.Web.Models
{
    public class ProductReviewsViewModel
    {
        public PagedList<ReviewDto> Reviews { get; set; } = new PagedList<ReviewDto>();
        public decimal AverageRating { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
    }
}