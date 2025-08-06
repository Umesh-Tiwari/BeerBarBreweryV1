using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.BeerBarBrewery
{
    /// <summary>
    /// Response DTO for a brewery with basic information.
    /// Typically used in scenarios where only minimal brewery data is needed (e.g., after create/update).
    /// </summary>
    public class BreweryResponse
    {
        /// <summary>
        /// Unique identifier of the brewery.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the brewery.
        /// </summary>
        public string Name { get; set; }
    }

}
