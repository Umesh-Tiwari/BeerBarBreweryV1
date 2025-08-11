namespace BeerBarBrewery.Middleware
{
    /// <summary>
    /// Custom exception class for handling "Not Found" scenarios in the application.
    /// Used to represent cases where a requested resource does not exist.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NotFoundException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotFoundException class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotFoundException class with a default error message.
        /// </summary>
        public NotFoundException() : base("The requested resource was not found.")
        {
        }
    }
}
