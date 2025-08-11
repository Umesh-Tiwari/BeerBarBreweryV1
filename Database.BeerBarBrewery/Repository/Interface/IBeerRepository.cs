using Database.Entities;

namespace Database.BeerBarBrewery.Repository.Interface
{
    /// <summary>
    /// Repository interface for managing Beer entities.
    /// Provides methods for CRUD operations and custom queries.
    /// </summary>
    public interface IBeerRepository
    {
        /// <summary>
        /// Retrieves all beers from the data source.
        /// </summary>
        /// <returns>A collection of all beers.</returns>
        Task<IEnumerable<Beer>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific beer by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the beer to retrieve.</param>
        /// <returns>The beer if found; otherwise, null.</returns>
        Task<Beer?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new beer to the data source.
        /// </summary>
        /// <param name="beer">The beer entity to add.</param>
        Task AddAsync(Beer beer);

        /// <summary>
        /// Updates an existing beer in the data source.
        /// </summary>
        /// <param name="beer">The beer entity with updated values.</param>
        void Update(Beer beer);

        /// <summary>
        /// Deletes a beer from the data source.
        /// </summary>
        /// <param name="beer">The beer entity to delete.</param>
        void Delete(Beer beer);

        /// <summary>
        /// Persists all changes made in the repository to the data source.
        /// </summary>
        /// <returns>True if changes were saved successfully; otherwise, false.</returns>
        Task<bool> SaveChangesAsync();

        /// <summary>
        /// Retrieves beers based on alcohol by volume (ABV) criteria.
        /// </summary>
        /// <param name="minAbv">Optional minimum ABV value. If provided, returns beers with ABV greater than this value.</param>
        /// <param name="maxAbv">Optional maximum ABV value. If provided, returns beers with ABV less than this value.</param>
        /// <returns>A collection of beers matching the ABV criteria.</returns>
        Task<IEnumerable<Beer>> GetBeersByAlcoholVolumeRangeAsync(decimal? minAbv, decimal? maxAbv);
    }
}
