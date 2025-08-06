using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.BeerBarBrewery.Repository
{
    /// <summary>
    /// Repository implementation for managing Brewery entities and their associated Beers.
    /// Provides standard CRUD operations and eager loading of related data.
    /// </summary>
    public class BreweryRepository : IBreweryRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreweryRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public BreweryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all breweries from the database.
        /// </summary>
        /// <returns>A collection of all breweries.</returns>
        public async Task<IEnumerable<Brewery>> GetAllAsync()
        {
            return await _context.Breweries.ToListAsync();
        }

        /// <summary>
        /// Retrieves all breweries along with their associated beers.
        /// </summary>
        /// <returns>A collection of breweries with their beers included.</returns>
        public async Task<IEnumerable<Brewery>> GetAllWithBeerAsync()
        {
            return await _context.Breweries
                .Include(b => b.Beers)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific brewery by its unique identifier, including its beers.
        /// </summary>
        /// <param name="id">The ID of the brewery to retrieve.</param>
        /// <returns>The brewery with its beers if found; otherwise, null.</returns>
        public async Task<Brewery?> GetByIdAsync(int id)
        {
            return await _context.Breweries
                .Include(b => b.Beers)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// Adds a new brewery to the database context.
        /// </summary>
        /// <param name="brewery">The brewery entity to add.</param>
        public async Task AddAsync(Brewery brewery)
        {
            await _context.Breweries.AddAsync(brewery);
        }

        /// <summary>
        /// Updates an existing brewery in the database context.
        /// </summary>
        /// <param name="brewery">The brewery entity with updated information.</param>
        public void Update(Brewery brewery)
        {
            _context.Breweries.Update(brewery);
        }

        /// <summary>
        /// Deletes a brewery from the database context.
        /// </summary>
        /// <param name="brewery">The brewery entity to delete.</param>
        public void Delete(Brewery brewery)
        {
            _context.Breweries.Remove(brewery);
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
