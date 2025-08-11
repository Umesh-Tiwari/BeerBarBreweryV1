namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a bar.
    /// Used for returning bar details to the client.
    /// </summary>
    public class BarResponse
    {
        /// <summary>
        /// Unique identifier of the bar.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the bar.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address/location of the bar.
        /// </summary>
        public string Address { get; set; }
    }
}
