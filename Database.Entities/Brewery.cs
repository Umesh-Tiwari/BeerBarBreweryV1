namespace Database.Entities
{
    /// <summary>
    /// Represents a Brewery entity that produces beers.
    /// </summary>
    public class Brewery
    {
        /// <summary>
        /// Gets or sets the unique identifier for the brewery.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the brewery.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Navigation property representing the collection of beers produced by the brewery.
        /// </summary>
        public ICollection<Beer> Beers { get; set; } = new List<Beer>();
    }
}
