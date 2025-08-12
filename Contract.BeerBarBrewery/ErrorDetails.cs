namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Represents error details returned in case of a failure
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Descriptive error message returned to the client
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// HTTP status code associated with the error
        /// </summary>
        public int StatusCode { get; set; }
    }
}
