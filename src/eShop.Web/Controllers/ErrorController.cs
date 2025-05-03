using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            string message;
            switch (statusCode)
            {
                case 404:
                    message = "Sorry, the page you’re looking for can’t be found.";
                    break;
                case 403:
                    message = "You don’t have permission to access this page.";
                    break;
                case 500:
                    message = "Something went wrong on our end.";
                    break;
                default:
                    message = "An unexpected error occurred.";
                    break;
            }
            _logger.LogWarning($"HTTP error {statusCode} occurred.");
            ViewBag.ErrorMessage = message;
            return View("Error");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature is not null)
            {
                _logger.LogError(exceptionFeature.Error, "Unhandled exception occurred.");
                ViewBag.ErrorMessage = "An unexpected error occurred. We're working on it!";
            }
            return View("Error");
        }

        [Route("Error/AuthFailed")]
        public IActionResult AuthFailed()
        {
            _logger.LogWarning("Authentication failed.");
            ViewBag.ErrorMessage = "Authentication failed. Please check your credentials and try again.";
            return View("Error");
        }
    }
}