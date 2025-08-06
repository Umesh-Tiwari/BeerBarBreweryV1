
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
        /// Gets or sets the optional foreign key for the associated brewery.
        /// Nullable to allow beers without an initial brewery assignment.
        /// </summary>
        public int? BreweryId { get; set; }

        /// <summary>
        /// Navigation property to the brewery that produced the beer.
        /// </summary>
        public Brewery Brewery { get; set; }

        /// <summary>
        /// Navigation property representing the bars that serve this beer via the BarBeer join entity.
        /// </summary>
        public ICollection<BarBeer> BarBeers { get; set; }
    }

}
