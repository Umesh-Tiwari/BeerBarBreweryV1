using Database.Entities;

namespace Database.BeerBarBrewery.Repository.Interface
{
    /// <summary>
    /// Repository interface for managing Bar entities in the data access layer.
    /// Provides methods for CRUD operations and managing beer associations.
    /// </summary>
    public interface IBarRepository
    {
        /// <summary>
        /// Retrieves all bars from the data store.
        /// </summary>
        Task<IEnumerable<Bar>> GetAllAsync();

        /// <summary>
        /// Retrieves a bar by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the bar.</param>
        /// <returns>The Bar entity if found; otherwise, null.</returns>
        Task<Bar?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new bar to the data store.
        /// </summary>
        /// <param name="bar">The Bar entity to add.</param>
        Task AddAsync(Bar bar);

        /// <summary>
        /// Updates an existing bar in the data store.
        /// </summary>
        /// <param name="bar">The updated Bar entity.</param>
        void Update(Bar bar);

        /// <summary>
        /// Deletes a bar from the data store.
        /// </summary>
        /// <param name="bar">The Bar entity to delete.</param>
        void Delete(Bar bar);

        /// <summary>
        /// Persists changes to the data store.
        /// </summary>
        /// <returns>True if changes were saved; otherwise, false.</returns>
        Task<bool> SaveChangesAsync();

        /// <summary>
        /// Links a beer to a bar by creating an association between them.
        /// </summary>
        /// <param name="barId">The ID of the bar.</param>
        /// <param name="beerId">The ID of the beer.</param>
        /// <returns>True if new relationship created, false if already exists.</returns>
        Task<bool> AssignBeerAsync(int barId, int beerId);

        /// <summary>
        /// Retrieves all beers served at a specific bar.
        /// </summary>
        /// <param name="barId">The ID of the bar.</param>
        /// <returns>A list of Beer entities associated with the bar.</returns>
        Task<IEnumerable<Beer>> GetBeersServedAtBarAsync(int barId);

        /// <summary>
        /// Retrieves all bars along with the beers they serve (eager loading).
        /// </summary>
        /// <returns>A list of Bar entities including associated beers.</returns>
        Task<IEnumerable<Bar>> GetAllBarsWithBeersAsync();
    }
}
