
namespace Database.Entities
{
    /// <summary>
    /// Represents the join entity for the many-to-many relationship between Bar and Beer.
    /// Each instance links a specific bar with a specific beer it serves.
    /// </summary>
    public class BarBeer
    {
        /// <summary>
        /// Gets or sets the foreign key for the associated Bar.
        /// </summary>
        public int BarId { get; set; }

        /// <summary>
        /// Navigation property to the associated Bar.
        /// </summary>
        public Bar Bar { get; set; }

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
