
namespace Database.Entities
{
    /// <summary>
    /// Represents a Bar entity that serves various beers.
    /// </summary>
    public class Bar
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bar.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the bar.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the bar.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Navigation property representing the beers served at this bar via the BarBeer join entity.
        /// </summary>
        public ICollection<BarBeer> BarBeers { get; set; } = new List<BarBeer>();
    }
}
