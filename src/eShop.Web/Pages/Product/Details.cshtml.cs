using eShop.Shared.DTOs.Products;
using eShop.Shared.DTOs.Reviews;
using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Web.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IApiClientWrapper _apiClientWrapper;
        private readonly IReviewService _reviewService;

        public ProductDetailDto? Product { get; private set; }

        [BindProperty]
        public CreateReviewDto NewReview { get; set; } = new();

        public DetailsModel(
            IReviewService reviewService,
            IApiClientWrapper apiClientWrapper
        )
        {
            _reviewService = reviewService;
            _apiClientWrapper = apiClientWrapper;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _apiClientWrapper.GetAsync<ProductDetailDto>($"products/details/{id}", requiresAuth: false);
            // if (Product is null)
            // {
            //     return NotFound();
            // }

            return Page();
        }

        public async Task<IActionResult> OnPostSubmitReviewAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                Product = await _apiClientWrapper.GetAsync<ProductDetailDto>($"products/details/{id}", requiresAuth: false);
                return Page();
            }

            try {
                await _reviewService.AddReviewAsync(id, NewReview);
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error submitting review: {ex.Message}");

                Product = await _apiClientWrapper.GetAsync<ProductDetailDto>($"products/details/{id}", requiresAuth: false);
                return Page();
            }
        }
    }
}