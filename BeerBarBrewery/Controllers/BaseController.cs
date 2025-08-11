using Contract.BeerBarBrewery;
using Microsoft.AspNetCore.Mvc;

namespace BeerBarBrewery.Controllers
{
    /// <summary>
    /// Base controller class that provides common functionality for all API controllers.
    /// Includes utility methods for validation, error handling, and standardized response formatting.
    /// All derived controllers inherit JSON response format and shared helper methods.
    /// </summary>
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Validates that an ID is greater than 0.
        /// </summary>
        /// <param name="id">The ID to validate.</param>
        /// <returns>True if the ID is valid (greater than 0); otherwise, false.</returns>
        protected bool IsValidId(int id) => id > 0;

        /// <summary>
        /// Creates a standardized error response object.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        /// <param name="statusCode">The HTTP status code (default: 400).</param>
        /// <returns>An ErrorDetails object with the specified message and status code.</returns>
        protected ErrorDetails ErrorResponse(string message, int statusCode = 400)
        {
            return new ErrorDetails
            {
                Message = message,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Creates an ActionResult with error response and specified status code.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code (default: 400).</param>
        /// <returns>An ActionResult with the error response.</returns>
        protected ActionResult ErrorResponseActionResult(string message, int statusCode = 400)
        {
            return StatusCode(statusCode, new { error = message });
        }

        /// <summary>
        /// Creates a standardized NotFound (404) response with error message.
        /// </summary>
        /// <param name="message">The error message to include.</param>
        /// <returns>A NotFound ActionResult with the error message.</returns>
        protected IActionResult NotFoundResponse(string message)
        {
            return NotFound(new { error = message });
        }

        /// <summary>
        /// Creates a standardized NotFound (404) action response with error message.
        /// </summary>
        /// <param name="message">The error message to include.</param>
        /// <returns>A NotFound ActionResult with the error message.</returns>
        protected IActionResult NotFoundActionResponse(string message)
        {
            return NotFound(new { error = message });
        }
    }
}
