using Contract.BeerBarBrewery;
using Model.BeerBarBrewery;

namespace Business.BeerBarBrewery.Process.Interface
{
    /// <summary>
    /// Interface defining the business logic operations for managing bars and their relationship with beers.
    /// </summary>
    public interface IBarProcess
    {
        /// <summary>
        /// Links an existing beer to a specified bar.
        /// </summary>
        /// <param name="barBeerLinkDto">DTO containing BarId and BeerId for linking.</param>
        /// <returns>True if link is successful; false if bar or beer not found.</returns>
        Task<bool> LinkBarToBeer(BarBeerRequest barBeerLinkDto);

        /// <summary>
        /// Retrieves all beers served at a specified bar.
        /// </summary>
        /// <param name="barId">The ID of the bar.</param>
        /// <returns>List of beers served at the bar.</returns>
        Task<IEnumerable<BeerModel>> GetBeersServedAtBarAsync(int barId);

        /// <summary>
        /// Retrieves all bars without their associated beers.
        /// </summary>
        /// <returns>List of all bars.</returns>
        Task<IEnumerable<BarModel>> GetAllBars();

        /// <summary>
        /// Retrieves all bars including their associated beers.
        /// </summary>
        /// <returns>List of all bars with beers served.</returns>
        Task<IEnumerable<BarModel>> GetAllBarsWithBeers();

        /// <summary>
        /// Retrieves a single bar by its ID.
        /// </summary>
        /// <param name="id">Bar ID to retrieve.</param>
        /// <returns>The bar model if found; null otherwise.</returns>
        Task<BarModel> GetBarById(int id);

        /// <summary>
        /// Creates a new bar using provided data.
        /// </summary>
        /// <param name="createBarModel">Model containing new bar information.</param>
        /// <returns>The created bar model with its assigned ID.</returns>
        Task<BarModel> CreateBar(CreateBarModel createBarModel);

        /// <summary>
        /// Updates an existing bar identified by ID.
        /// </summary>
        /// <param name="id">ID of the bar to update.</param>
        /// <param name="createBarModel">Model with updated bar data.</param>
        /// <returns>True if update is successful; false if bar not found.</returns>
        Task<bool> UpdateBar(int id, CreateBarModel createBarModel);

        /// <summary>
        /// Deletes a bar identified by ID.
        /// </summary>
        /// <param name="id">ID of the bar to delete.</param>
        /// <returns>True if deletion is successful; false if bar not found.</returns>
        Task<bool> DeleteBar(int id);
    }

}
