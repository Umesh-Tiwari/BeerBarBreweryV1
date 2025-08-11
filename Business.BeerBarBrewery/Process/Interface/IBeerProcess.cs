using Model.BeerBarBrewery;

namespace Business.BeerBarBrewery.Process.Interface
{
    /// <summary>
    /// Interface defining the business logic operations for managing beers.
    /// </summary>
    public interface IBeerProcess
    {
        /// <summary>
        /// Retrieves all beers from the system.
        /// </summary>
        /// <returns>A list of all beers.</returns>
        Task<IEnumerable<BeerModel>> GetAllBeers();

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
        /// Retrieves beers that fall within the specified alcohol by volume (ABV) range.
        /// </summary>
        /// <param name="minAbv">Minimum ABV (greater than or equal to).</param>
        /// <param name="maxAbv">Maximum ABV (less than or equal to).</param>
        /// <returns>List of beers within the given ABV range.</returns>
        Task<IEnumerable<BeerModel>> GetBeersByAlcoholVolumeRange(decimal minAbv, decimal maxAbv);

        /// <summary>
        /// Deletes a beer identified by its ID.
        /// </summary>
        /// <param name="id">The ID of the beer to delete.</param>
        /// <returns>True if the beer was deleted; false if not found.</returns>
        Task<bool> DeleteBeer(int id);
    }
}
