using System.ComponentModel.DataAnnotations;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Request model for assigning a beer to a brewery.
    /// Used to create associations between breweries and beers.
    /// </summary>
    public class BreweryBeerRequest
    {
        /// <summary>
        /// The ID of the brewery to assign the beer to.
        /// </summary>
        [Required(ErrorMessage = "Brewery ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Brewery ID must be greater than 0.")]
        public int BreweryId { get; set; }

        /// <summary>
        /// The ID of the beer to assign to the brewery.
        /// </summary>
        [Required(ErrorMessage = "Beer ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Beer ID must be greater than 0.")]
        public int BeerId { get; set; }
    }
}