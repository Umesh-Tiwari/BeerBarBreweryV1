using Database.BeerBarBrewery.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Database.BeerBarBrewery.Repository
{
    /// <summary>
    /// Generic repository implementation for standard CRUD operations.
    /// Works with any entity type using Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of entity the repository manages. Must be a class.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// The database context instance.
        /// </summary>
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// The DbSet representing the entity collection.
        /// </summary>
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();  // Dynamically gets the DbSet<T> for the entity
        }

        /// <summary>
        /// Retrieves all entities of type T from the database.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Retrieves a single entity by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Adds a new entity to the database context.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Updates an existing entity in the database context.
        /// </summary>
        /// <param name="entity">The entity with updated data.</param>
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Deletes an entity from the database context.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }

}
