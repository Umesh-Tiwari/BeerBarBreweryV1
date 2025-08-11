namespace Database.BeerBarBrewery.Repository.Interface
{
    /// <summary>
    /// Generic repository interface defining standard CRUD operations for any entity type.
    /// </summary>
    /// <typeparam name="T">The type of entity the repository will manage. Must be a class.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities of type T from the data source.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity if found; otherwise, null (depending on implementation).</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new entity to the data source.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the data source.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes an entity from the data source.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);
    }
}
