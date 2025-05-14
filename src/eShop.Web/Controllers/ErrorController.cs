using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eShop.Web.Exceptions;
using eShop.Web.Models;

namespace eShop.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorController(ILogger<ErrorController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var viewModel = new ErrorViewModel
            {
                StatusCode = statusCode,
                RequestId = HttpContext.TraceIdentifier
            };

            switch (statusCode)
            {
                case 404:
                    viewModel.Title = "Page Not Found";
                    viewModel.Message = "Sorry, the page you're looking for can't be found.";
                    break;
                case 403:
                    viewModel.Title = "Access Denied";
                    viewModel.Message = "You don't have permission to access this page.";
                    break;
                case 500:
                    viewModel.Title = "Server Error";
                    viewModel.Message = "Something went wrong on our end.";
                    break;
                default:
                    viewModel.Title = "Error";
                    viewModel.Message = "An unexpected error occurred.";
                    break;
            }

            _logger.LogWarning("HTTP error {StatusCode} occurred for {Path}",
                statusCode, HttpContext.Request.Path);

            return View("Error", viewModel);
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionFeature?.Error;

            var viewModel = new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier,
                Title = "Error"
            };

            if (exception == null)
            {
                viewModel.Message = "An unexpected error occurred.";
                return View("Error", viewModel);
            }

            // Handle our custom API exceptions
            if (exception is ApiException apiException)
            {
                viewModel.StatusCode = apiException.StatusCode;

                if (exception is NotFoundException)
                {
                    viewModel.Title = "Not Found";
                    viewModel.Message = apiException.Message;
                }
                else if (exception is UnauthorizedException)
                {
                    viewModel.Title = "Authentication Required";
                    viewModel.Message = "Please sign in to access this resource.";
                }
                else if (exception is ForbiddenException)
                {
                    viewModel.Title = "Access Denied";
                    viewModel.Message = "You don't have permission to access this resource.";
                }
                else if (exception is BadRequestException)
                {
                    viewModel.Title = "Invalid Request";
                    viewModel.Message = apiException.Message;
                }
                else
                {
                    viewModel.Title = "Service Error";
                    viewModel.Message = "Something went wrong while processing your request.";
                }
            }
            else
            {
                viewModel.Title = "Application Error";
                viewModel.Message = "An unexpected error occurred. We're working on it!";
                viewModel.StatusCode = 500;
            }

            // Add technical details in development
            if (_env.IsDevelopment())
            {
                viewModel.DeveloperMessage = exception.ToString();
            }

            _logger.LogError(exception, "Unhandled exception for {Path}", HttpContext.Request.Path);

            return View("Error", viewModel);
        }

        [Route("Error/AuthFailed")]
        public IActionResult AuthFailed()
        {
            _logger.LogWarning("Authentication failed for {Path}", HttpContext.Request.Path);

            var viewModel = new ErrorViewModel
            {
                Title = "Authentication Failed",
                Message = "Authentication failed. Please check your credentials and try again.",
                RequestId = HttpContext.TraceIdentifier
            };

            return View("Error", viewModel);
        }
    }
}