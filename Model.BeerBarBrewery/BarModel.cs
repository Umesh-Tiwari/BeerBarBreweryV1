namespace Model.BeerBarBrewery
{

    /// <summary>
    /// Represents the business model for a bar entity used in service/business layers.
    /// </summary>
    public class BarModel
    {
        /// <summary>
        /// Unique identifier for the bar.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the bar.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Physical address of the bar.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// List of beers served at this bar.
        /// </summary>
        public List<BeerModel>? Beers { get; set; }
    }
}
