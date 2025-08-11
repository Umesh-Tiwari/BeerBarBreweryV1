namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a beer.
    /// Used for returning beer details to the client.
    /// </summary>
    public class BeerResponse
    {
        /// <summary>
        /// Unique identifier of the beer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the beer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alcohol content of the beer, expressed as a percentage (e.g., 5.5%).
        /// </summary>
        public decimal PercentageAlcoholByVolume { get; set; }

        ///// <summary>
        ///// Optional ID of the brewery that produces the beer. Null if not associated.
        ///// </summary>
        public int? BreweryId { get; set; }
    }

}
