namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Business model for representing brewery-beer associations.
    /// Used in the service layer for managing brewery-beer relationships.
    /// </summary>
    public class BreweryBeerModel
    {
        /// <summary>
        /// The ID of the brewery.
        /// </summary>
        public int BreweryId { get; set; }

        /// <summary>
        /// The ID of the beer.
        /// </summary>
        public int BeerId { get; set; }
    }
}