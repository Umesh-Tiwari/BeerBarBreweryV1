namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents the business model for a brewery entity used in the service/business layer.
    /// </summary>
    public class BreweryModel
    {
        /// <summary>
        /// Unique identifier for the brewery.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the brewery.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of beers produced by this brewery.
        /// Nullable to allow flexibility when beer data is not needed.
        /// </summary>
        public List<BeerModel>? Beers { get; set; }
    }
}
