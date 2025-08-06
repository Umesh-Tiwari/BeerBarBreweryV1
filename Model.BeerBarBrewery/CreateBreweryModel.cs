using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Name { get; set; }
    }

}
