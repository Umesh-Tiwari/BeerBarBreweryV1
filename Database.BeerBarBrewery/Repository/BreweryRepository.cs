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
                .Include(b => b.BreweryBeers)
                    .ThenInclude(bb => bb.Beer)
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
                .Include(b => b.BreweryBeers)
                    .ThenInclude(bb => bb.Beer)
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
        /// Assigns a beer to a brewery if not already assigned.
        /// </summary>
        /// <param name="breweryId">The ID of the brewery.</param>
        /// <param name="beerId">The ID of the beer to assign.</param>
        /// <returns>True if new relationship created, false if already exists.</returns>
        public async Task<bool> AssignBeerAsync(int breweryId, int beerId)
        {
            var exists = await _context.BreweryBeers
                .AnyAsync(bb => bb.BreweryId == breweryId && bb.BeerId == beerId);

            if (!exists)
            {
                await _context.BreweryBeers.AddAsync(new BreweryBeer { BreweryId = breweryId, BeerId = beerId });
                return true;
            }
            return false;
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
