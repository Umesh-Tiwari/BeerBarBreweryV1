namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents a model for linking a beer to a brewery in the business layer.
    /// Used for assigning or updating the relationship between beers and breweries.
    /// </summary>
    public class BreweryBeerModel
    {
        /// <summary>
        /// Identifier of the brewery to which the beer will be assigned.
        /// </summary>
        public int BreweryId { get; set; }

        /// <summary>
        /// Identifier of the beer that is being assigned to a brewery.
        /// </summary>
        public int BeerId { get; set; }
    }
}
