namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents the result of assigning a beer to a brewery or bar.
    /// </summary>
    public enum AssignmentResult
    {
        /// <summary>
        /// The assignment was successful and a new relationship was created.
        /// </summary>
        Success,
        
        /// <summary>
        /// The relationship already exists.
        /// </summary>
        AlreadyExists,
        
        /// <summary>
        /// The brewery or beer was not found.
        /// </summary>
        NotFound
    }
}