using System.ComponentModel.DataAnnotations;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Brewery beer object for linking a beer to a brewery.
    /// Used to associate an existing beer with a specific brewery.
    /// </summary>
    public class BreweryWithBeerRequest
    {
        /// <summary>
        /// The ID of the brewery producing the beer.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Valid brewery ID is required.")]
        public int BreweryId { get; set; }

        /// <summary>
        /// The ID of the beer to be linked to the brewery.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Valid beer ID is required.")]
        public int BeerId { get; set; }
    }
}
