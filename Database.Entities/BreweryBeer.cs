namespace Database.Entities
{
    /// <summary>
    /// Represents the join entity for the many-to-many relationship between Brewery and Beer.
    /// Each instance links a specific brewery with a specific beer it produces.
    /// </summary>
    public class BreweryBeer
    {
        /// <summary>
        /// Gets or sets the foreign key for the associated Brewery.
        /// </summary>
        public int BreweryId { get; set; }

        /// <summary>
        /// Navigation property to the associated Brewery.
        /// </summary>
        public Brewery Brewery { get; set; }

        /// <summary>
        /// Gets or sets the foreign key for the associated Beer.
        /// </summary>
        public int BeerId { get; set; }

        /// <summary>
        /// Navigation property to the associated Beer.
        /// </summary>
        public Beer Beer { get; set; }
    }
}