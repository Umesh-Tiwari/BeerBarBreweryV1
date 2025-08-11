using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.BeerBarBrewery.Repository
{
    /// <summary>
    /// Repository implementation for managing Beer entities.
    /// Provides methods for standard CRUD operations and custom queries.
    /// </summary>
    public class BeerRepository : IBeerRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeerRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public BeerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all beers from the database.
        /// </summary>
        public async Task<IEnumerable<Beer>> GetAllAsync()
        {
            return await _context.Beers.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific beer by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the beer to retrieve.</param>
        /// <returns>The beer if found; otherwise, null.</returns>
        public async Task<Beer?> GetByIdAsync(int id)
        {
            return await _context.Beers.FindAsync(id);
        }

        /// <summary>
        /// Adds a new beer to the database context.
        /// </summary>
        /// <param name="beer">The beer entity to add.</param>
        public async Task AddAsync(Beer beer)
        {
            await _context.Beers.AddAsync(beer);
        }

        /// <summary>
        /// Updates an existing beer in the database context.
        /// </summary>
        /// <param name="beer">The beer entity with updated information.</param>
        public void Update(Beer beer)
        {
            _context.Beers.Update(beer);
        }

        /// <summary>
        /// Deletes a beer from the database context.
        /// </summary>
        /// <param name="beer">The beer entity to delete.</param>
        public void Delete(Beer beer)
        {
            _context.Beers.Remove(beer);
        }

        /// <summary>
        /// Retrieves beers based on alcohol by volume (ABV) criteria.
        /// </summary>
        /// <param name="minAbv">Optional minimum ABV value. If provided, returns beers with ABV greater than this value.</param>
        /// <param name="maxAbv">Optional maximum ABV value. If provided, returns beers with ABV less than this value.</param>
        /// <returns>A collection of beers matching the ABV criteria.</returns>
        public async Task<IEnumerable<Beer>> GetBeersByAlcoholVolumeRangeAsync(decimal? minAbv, decimal? maxAbv)
        {
            var query = _context.Beers.AsQueryable();

            if (minAbv.HasValue)
                query = query.Where(b => b.PercentageAlcoholByVolume > minAbv.Value);

            if (maxAbv.HasValue)
                query = query.Where(b => b.PercentageAlcoholByVolume < maxAbv.Value);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Saves all changes made in the context to the database.
        /// </summary>
        /// <returns>True if changes were saved successfully; otherwise, false.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
