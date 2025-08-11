using Contract.BeerBarBrewery;
using Microsoft.AspNetCore.Mvc;

namespace BeerBarBrewery.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected bool IsValidId(int id) => id > 0;

        protected ErrorDetails ErrorResponse(string message, int statusCode = 400)
        {
            return new ErrorDetails
            {
                Message = message,
                StatusCode = statusCode
            };
        }

        protected ActionResult ErrorResponseActionResult(string message, int statusCode = 400)
        {
            return StatusCode(statusCode, new { error = message });
        }

        protected IActionResult NotFoundResponse(string message)
        {
            return NotFound(new { error = message });
        }

        protected IActionResult NotFoundActionResponse(string message)
        {
            return NotFound(new { error = message });
        }
    }
}
