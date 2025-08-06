using Database.Entities;

namespace Database.BeerBarBrewery.Repository.Interface
{
    /// <summary>
    /// Repository interface for managing Brewery entities.
    /// Inherits basic CRUD operations from IGenericRepository.
    /// </summary>
    public interface IBreweryRepository
    {
        /// <summary>
        /// Retrieves all breweries from the data source.
        /// </summary>
        /// <returns>A collection of all breweries.</returns>
        Task<IEnumerable<Brewery>> GetAllAsync();

        /// <summary>
        /// Retrieves all breweries along with their associated beers.
        /// </summary>
        /// <returns>A collection of breweries with their beers included.</returns>
        Task<IEnumerable<Brewery>> GetAllWithBeerAsync();

        /// <summary>
        /// Retrieves a specific brewery by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the brewery to retrieve.</param>
        /// <returns>The brewery if found; otherwise, null.</returns>
        Task<Brewery?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new brewery to the data source.
        /// </summary>
        /// <param name="brewery">The brewery entity to add.</param>
        Task AddAsync(Brewery brewery);

        /// <summary>
        /// Updates an existing brewery in the data source.
        /// </summary>
        /// <param name="brewery">The brewery entity with updated values.</param>
        void Update(Brewery brewery);

        /// <summary>
        /// Deletes a brewery from the data source.
        /// </summary>
        /// <param name="brewery">The brewery entity to delete.</param>
        void Delete(Brewery brewery);

        /// <summary>
        /// Persists all changes made in the repository to the data source.
        /// </summary>
        /// <returns>True if changes were saved successfully; otherwise, false.</returns>
        Task<bool> SaveChangesAsync();
    }

}
