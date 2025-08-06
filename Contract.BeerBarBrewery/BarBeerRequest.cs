using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int BarId { get; set; }

        /// <summary>
        /// The ID of the beer to be linked to the bar.
        /// </summary>
        [Required]
        public int BeerId { get; set; }
    }

}
