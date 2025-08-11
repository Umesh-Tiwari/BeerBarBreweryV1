namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents the business model for a beer entity used in service/business layers.
    /// </summary>
    public class BeerModel
    {
        /// <summary>
        /// Unique identifier for the beer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the beer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alcohol content represented as a percentage (ABV - Alcohol by Volume).
        /// </summary>
        public decimal PercentageAlcoholByVolume { get; set; }


    }
}
