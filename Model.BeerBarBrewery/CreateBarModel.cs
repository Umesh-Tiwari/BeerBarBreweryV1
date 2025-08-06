using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Name { get; set; }

        /// <summary>
        /// Address of the bar to be created.
        /// </summary>
        public string Address { get; set; }
    }

}
