using Model.BeerBarBrewery;

namespace Business.BeerBarBrewery.Process.Interface
{
    /// <summary>
    /// Interface defining the business logic operations for managing breweries and their beers.
    /// </summary>
    public interface IBreweryProcess
    {
        /// <summary>
        /// Retrieves all breweries in the system.
        /// </summary>
        /// <returns>A list of brewery models.</returns>
        Task<IEnumerable<BreweryModel>> GetAllBreweries();

        /// <summary>
        /// Retrieves all breweries along with their associated beers.
        /// </summary>
        /// <returns>A list of breweries with their beers.</returns>
        Task<IEnumerable<BreweryModel>> GetAllWithBeerAsync();

        /// <summary>
        /// Retrieves a specific brewery by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the brewery to retrieve.</param>
        /// <returns>The brewery model if found; otherwise null.</returns>
        Task<BreweryModel> GetBreweryById(int id);

        /// <summary>
        /// Creates a new brewery using the provided data.
        /// </summary>
        /// <param name="createBreweryModel">Model containing brewery creation data.</param>
        /// <returns>The created brewery model including its ID.</returns>
        Task<BreweryModel> CreateBrewery(CreateBreweryModel createBreweryModel);

        /// <summary>
        /// Updates an existing brewery identified by its ID.
        /// </summary>
        /// <param name="id">The ID of the brewery to update.</param>
        /// <param name="updateBreweryModel">Model containing updated brewery data.</param>
        /// <returns>True if update is successful; false if brewery not found.</returns>
        Task<bool> UpdateBrewery(int id, CreateBreweryModel updateBreweryModel);

        /// <summary>
        /// Links an existing beer to a specific brewery.
        /// </summary>
        /// <param name="breweryBeerModel">Model containing BreweryId and BeerId for linking.</param>
        /// <returns>AssignmentResult indicating success, already exists, or not found.</returns>
        Task<AssignmentResult> AssignBeerToBrewery(BreweryBeerModel breweryBeerModel);

        /// <summary>
        /// Deletes a brewery identified by its ID.
        /// </summary>
        /// <param name="id">The ID of the brewery to delete.</param>
        /// <returns>True if the brewery was deleted; false if not found.</returns>
        Task<bool> DeleteBrewery(int id);
    }
}
