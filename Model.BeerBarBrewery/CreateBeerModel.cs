using System.ComponentModel.DataAnnotations;

namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents the business model for creating a new beer.
    /// Used in the service layer to handle beer creation logic.
    /// </summary>
    public class CreateBeerModel
    {
        /// <summary>
        /// Name of the beer to be created.
        /// </summary>
        [Required(ErrorMessage = "Beer name is required.")]
        [StringLength(100, ErrorMessage = "Beer name cannot exceed 100 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Alcohol content of the beer represented as a percentage (ABV - Alcohol by Volume).
        /// </summary>
        [Range(0, 100, ErrorMessage = "Alcohol percentage must be between 0 and 100.")]
        public decimal PercentageAlcoholByVolume { get; set; }


    }
}
