using System.ComponentModel.DataAnnotations;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Beer bar id for linking a beer to a bar.
    /// Used to associate an existing Beer with an existing Bar.
    /// </summary>
    public class BarBeerRequest
    {
        /// <summary>
        /// The ID of the bar where the beer will be served.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Valid bar ID is required.")]
        public int BarId { get; set; }

        /// <summary>
        /// The ID of the beer to be linked to the bar.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Valid beer ID is required.")]
        public int BeerId { get; set; }
    }
}
