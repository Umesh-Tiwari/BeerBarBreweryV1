namespace Database.Entities
{
    /// <summary>
    /// Represents a Beer entity with its properties and relationships.
    /// </summary>
    public class Beer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the beer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the beer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the alcohol by volume (ABV) percentage of the beer.
        /// </summary>
        public decimal PercentageAlcoholByVolume { get; set; }

        /// <summary>
        /// Navigation property representing the bars that serve this beer via the BarBeer join entity.
        /// </summary>
        public ICollection<BarBeer> BarBeers { get; set; } = new List<BarBeer>();

        /// <summary>
        /// Navigation property representing the breweries that produce this beer via the BreweryBeer join entity.
        /// </summary>
        public ICollection<BreweryBeer> BreweryBeers { get; set; } = new List<BreweryBeer>();
    }
}
