using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// This class represent Bar with associated Beer information. It is a response object.
    /// </summary>
    public class BarWithBeerResponse
    {
        /// <summary>
        /// Unique identifier of the bar.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the bar.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address/location of the bar.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// List of beers served at the bar. Null if not requested.
        /// </summary>
        public List<BeerResponse>? Beers { get; set; }
    }
}
