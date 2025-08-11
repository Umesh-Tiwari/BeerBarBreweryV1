using System.ComponentModel.DataAnnotations;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Data Transfer Object (DTO) used to create a new bar.
    /// Captures basic input details such as name and address.
    /// </summary>
    public class CreateBarRequest
    {
        /// <summary>
        /// Name of the bar to be created.
        /// </summary>
        [Required(ErrorMessage = "Bar name is required.")]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Address or location of the bar.
        /// </summary>
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200)]
        public string Address { get; set; }
    }
}
