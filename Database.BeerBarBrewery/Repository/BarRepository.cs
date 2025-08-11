using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.BeerBarBrewery.Repository
{
    /// <summary>
    /// Repository implementation for managing Bar entities and their relationships with Beer entities.
    /// </summary>
    public class BarRepository : IBarRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public BarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all bars from the database.
        /// </summary>
        public async Task<IEnumerable<Bar>> GetAllAsync()
        {
            return await _context.Bars.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific bar by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the bar to retrieve.</param>
        /// <returns>The bar if found; otherwise, null.</returns>
        public async Task<Bar?> GetByIdAsync(int id)
        {
            return await _context.Bars.FindAsync(id);
        }

        /// <summary>
        /// Adds a new bar to the database context.
        /// </summary>
        /// <param name="bar">The bar entity to add.</param>
        public async Task AddAsync(Bar bar)
        {
            await _context.Bars.AddAsync(bar);
        }

        /// <summary>
        /// Updates an existing bar in the database context.
        /// </summary>
        /// <param name="bar">The bar entity with updated information.</param>
        public void Update(Bar bar)
        {
            _context.Bars.Update(bar);
        }

        /// <summary>
        /// Deletes a bar from the database context.
        /// </summary>
        /// <param name="bar">The bar entity to delete.</param>
        public void Delete(Bar bar)
        {
            _context.Bars.Remove(bar);
        }

        /// <summary>
        /// Assigns a beer to a bar if not already assigned.
        /// </summary>
        /// <param name="barId">The ID of the bar.</param>
        /// <param name="beerId">The ID of the beer to assign.</param>
        public async Task AssignBeerAsync(int barId, int beerId)
        {
            // Check if the beer is already assigned to the bar
            var exists = await _context.BarBeers
                .AnyAsync(bb => bb.BarId == barId && bb.BeerId == beerId);

            if (!exists)
            {
                // Create new association if it doesn't exist
                await _context.BarBeers.AddAsync(new BarBeer { BarId = barId, BeerId = beerId });
            }
        }

        /// <summary>
        /// Retrieves all beers served at a specific bar.
        /// </summary>
        /// <param name="barId">The ID of the bar.</param>
        /// <returns>A list of beers served at the specified bar.</returns>
        public async Task<IEnumerable<Beer>> GetBeersServedAtBarAsync(int barId)
        {
            return await _context.BarBeers
                .Where(bb => bb.BarId == barId)
                .Select(bb => bb.Beer)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all bars along with their associated beers using eager loading.
        /// </summary>
        /// <returns>A list of bars with their beers included.</returns>
        public async Task<IEnumerable<Bar>> GetAllBarsWithBeersAsync()
        {
            return await _context.Bars
                .Include(b => b.BarBeers)
                    .ThenInclude(bb => bb.Beer)
                .ToListAsync();
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
