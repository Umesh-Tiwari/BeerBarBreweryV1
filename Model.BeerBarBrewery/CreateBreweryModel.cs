using System.ComponentModel.DataAnnotations;

namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents the business model for creating a new brewery.
    /// Used in the service layer during brewery creation operations.
    /// </summary>
    public class CreateBreweryModel
    {
        /// <summary>
        /// Name of the brewery to be created.
        /// </summary>
        [Required(ErrorMessage = "Brewery name is required.")]
        [StringLength(100, ErrorMessage = "Brewery name cannot exceed 100 characters.")]
        public string Name { get; set; }
    }
}
