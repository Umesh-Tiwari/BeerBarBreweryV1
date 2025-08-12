using Model.BeerBarBrewery;

namespace Business.BeerBarBrewery.Process.Interface
{
    /// <summary>
    /// Interface defining the business logic operations for managing beers.
    /// </summary>
    public interface IBeerProcess
    {
        /// <summary>
        /// Retrieves a specific beer by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the beer to retrieve.</param>
        /// <returns>The beer model if found; otherwise null.</returns>
        Task<BeerModel> GetBeerById(int id);

        /// <summary>
        /// Creates a new beer using the provided data.
        /// </summary>
        /// <param name="createBeerModel">Model containing beer details to create.</param>
        /// <returns>The created beer model including its assigned ID.</returns>
        Task<BeerModel> CreateBeer(CreateBeerModel createBeerModel);

        /// <summary>
        /// Updates an existing beer identified by its ID.
        /// </summary>
        /// <param name="id">The ID of the beer to update.</param>
        /// <param name="updateModel">Model containing updated beer data.</param>
        /// <returns>True if the beer was updated; false if not found.</returns>
        Task<bool> UpdateBeer(int id, CreateBeerModel updateModel);

        /// <summary>
        /// Retrieves beers based on alcohol by volume (ABV) criteria.
        /// </summary>
        /// <param name="minAbv">Optional minimum ABV. If provided, returns beers with ABV greater than this value.</param>
        /// <param name="maxAbv">Optional maximum ABV. If provided, returns beers with ABV less than this value.</param>
        /// <returns>List of beers matching the ABV criteria.</returns>
        Task<IEnumerable<BeerModel>> GetBeersByAlcoholVolumeRange(decimal? minAbv, decimal? maxAbv);

        /// <summary>
        /// Deletes a beer identified by its ID.
        /// </summary>
        /// <param name="id">The ID of the beer to delete.</param>
        /// <returns>True if the beer was deleted; false if not found.</returns>
        Task<bool> DeleteBeer(int id);
    }
}
