using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.BeerBarBrewery
{
    /// <summary>
    /// Represents the business model for a beer entity used in service/business layers.
    /// </summary>
    public class BeerModel
    {
        /// <summary>
        /// Unique identifier for the beer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the beer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alcohol content represented as a percentage (ABV - Alcohol by Volume).
        /// </summary>
        public double PercentageAlcoholByVolume { get; set; }

        /// <summary>
        /// Optional reference to the brewery that produced this beer.
        /// </summary>
        public int? BreweryId { get; set; }
    }

}
