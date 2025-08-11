using System.ComponentModel.DataAnnotations;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// This class is used to create a new beer record to be saved.
    /// Captures input details such as name, alcohol content, and optional brewery association.
    /// </summary>
    public class CreateBeerRequest
    {
        /// <summary>
        /// Name of the beer to be created.
        /// </summary>
        [Required(ErrorMessage = "Beer name is required.")]
        [StringLength(100, ErrorMessage = "Beer name cannot exceed 100 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Alcohol content of the beer, expressed as a percentage (e.g., 5.5 for 5.5% ABV).
        /// </summary>
        [Required(ErrorMessage = "Alcohol content is required.")]
        [Range(0.1, 100.0, ErrorMessage = "Alcohol content must be between 0.1% and 100%.")]
        public double PercentageAlcoholByVolume { get; set; }

        /// <summary>
        /// Optional ID of the brewery producing this beer. Null if not linked at creation time.
        /// </summary>
        public int? BreweryId { get; set; }
    }
}
