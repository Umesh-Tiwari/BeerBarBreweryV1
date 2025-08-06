using System.ComponentModel.DataAnnotations;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Brewery request object used to create a new brewery.
    /// Captures the name of the brewery to be created.
    /// </summary>
    public class CreateBreweryRequest
    {
        /// <summary>
        /// Name of the brewery.
        /// </summary>
        [Required(ErrorMessage = "Brewery name is required.")]
        [StringLength(100, ErrorMessage = "Brewery name cannot exceed 100 characters.")]
        public string Name { get; set; }
    }

}
