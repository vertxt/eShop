using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestErrorsController : ControllerBase
    {
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            throw new KeyNotFoundException("The requested item was not found");
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            throw new ArgumentException("Invalid argument provided");
        }

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            throw new UnauthorizedAccessException("You don't have permission to access this resource");
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("Something went wrong on the server");
        }

        [HttpGet("validation")]
        public IActionResult GetValidationError()
        {
            ModelState.AddModelError("Username", "Username should be at least 5 characters");
            ModelState.AddModelError("Email", "Invalid email format");

            return ValidationProblem(ModelState);
        }
    }
}
