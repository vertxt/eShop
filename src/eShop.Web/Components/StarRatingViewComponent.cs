using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Components
{
    public class StarRatingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(decimal rating, int totalReviews = 0, bool showCount = true, string cssClass = "")
        {
            ViewBag.Rating = rating;
            ViewBag.TotalReviews = totalReviews;
            ViewBag.ShowCount = showCount;
            ViewBag.CssClass = cssClass;

            return View();
        }
    }
}