using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace BeerBarBrewery.Middleware
{
    /// <summary>
    /// Middleware for global exception handling.
    /// Catches unhandled exceptions, logs them, and returns a standardized JSON error response.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Constructor injecting the next middleware in the pipeline and the logger.
        /// </summary>
        /// <param name="next">Next middleware delegate.</param>
        /// <param name="logger">Logger for error logging.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Middleware invoke method that wraps request execution and handles exceptions.
        /// </summary>
        /// <param name="context">HTTP context for the current request.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Proceed to the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the unhandled exception
                _logger.LogError(ex, "Unhandled exception occurred.");

                // Handle exception and return appropriate JSON response
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Maps exceptions to HTTP status codes and writes a JSON response.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <param name="exception">Caught exception.</param>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Determine HTTP status code based on exception type
            int statusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,              // 404
                ValidationException => (int)HttpStatusCode.BadRequest,         // 400
                _ => (int)HttpStatusCode.InternalServerError                   // 500
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Prepare JSON error response payload
            var response = new
            {
                StatusCode = statusCode,
                Message = exception.Message
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }

}
