using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int BreweryId { get; set; }

        /// <summary>
        /// The ID of the beer to be linked to the brewery.
        /// </summary>
        public int BeerId { get; set; }
    }

}
