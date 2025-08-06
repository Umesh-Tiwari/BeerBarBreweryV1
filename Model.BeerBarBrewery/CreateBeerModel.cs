using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents the business model for creating a new beer.
    /// Used in the service layer to handle beer creation logic.
    /// </summary>
    public class CreateBeerModel
    {
        /// <summary>
        /// Name of the beer to be created.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alcohol content of the beer represented as a percentage (ABV - Alcohol by Volume).
        /// </summary>
        public double PercentageAlcoholByVolume { get; set; }

        /// <summary>
        /// Optional ID of the brewery that produces this beer.
        /// Can be null if not yet assigned.
        /// </summary>
        public int? BreweryId { get; set; }
    }

}
