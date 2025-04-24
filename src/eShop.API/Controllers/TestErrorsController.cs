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

        [HttpGet("divide-by-zero")]
        public IActionResult GetDivideByZero()
        {
            var result = 1 / Convert.ToInt32("0");
            return Ok(result);
        }
        
        [HttpGet("not-found-response")]
        public IActionResult GetNotFoundResponse()
        {
            return NotFound();
        }

        [HttpGet("bad-request-response")]
        public IActionResult GetBadRequestResponse()
        {
            return BadRequest();
        }
    }
}