using System.ComponentModel.DataAnnotations;

namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents the business model for creating a new bar.
    /// Used in the service layer when handling bar creation logic.
    /// </summary>
    public class CreateBarModel
    {
        /// <summary>
        /// Name of the bar to be created.
        /// </summary>
        [Required(ErrorMessage = "Bar name is required.")]
        [StringLength(100, ErrorMessage = "Bar name cannot exceed 100 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Address of the bar to be created.
        /// </summary>
        [Required(ErrorMessage = "Bar address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }
    }
}
