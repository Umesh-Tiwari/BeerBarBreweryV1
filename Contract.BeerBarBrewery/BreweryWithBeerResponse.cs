using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a brewery.
    /// Used for returning brewery details, including associated beers.
    /// </summary>
    public class BreweryWithBeerResponse
    {
        /// <summary>
        /// Unique identifier of the brewery.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the brewery.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of beers produced by the brewery. Can be null if beers are not included in the response.
        /// </summary>
        public List<BeerResponse>? Beers { get; set; }
    }

}
