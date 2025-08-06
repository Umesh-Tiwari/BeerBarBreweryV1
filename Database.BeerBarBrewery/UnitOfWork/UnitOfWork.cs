using Database.BeerBarBrewery.Repository.Interface;
using Database.BeerBarBrewery.UnitOfWork.Interface;

namespace Database.BeerBarBrewery.UnitOfWork
{
    /// <summary>
    /// Implementation of the Unit of Work pattern.
    /// Coordinates repository operations using a shared ApplicationDbContext instance.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Gets the repository for Brewery entities.
        /// </summary>
        public IBreweryRepository Breweries { get; }

        /// <summary>
        /// Gets the repository for Beer entities.
        /// </summary>
        public IBeerRepository Beers { get; }

        /// <summary>
        /// Gets the repository for Bar entities.
        /// </summary>
        public IBarRepository Bars { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="breweryRepository">Injected brewery repository.</param>
        /// <param name="beerRepository">Injected beer repository.</param>
        /// <param name="barRepository">Injected bar repository.</param>
        public UnitOfWork(ApplicationDbContext context,
                          IBreweryRepository breweryRepository,
                          IBeerRepository beerRepository,
                          IBarRepository barRepository)
        {
            _context = context;
            Breweries = breweryRepository;
            Beers = beerRepository;
            Bars = barRepository;
        }

        /// <summary>
        /// Commits all changes made through the repositories to the database.
        /// </summary>
        /// <returns>True if one or more changes were saved; otherwise, false.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        /// <summary>
        /// Disposes the underlying database context.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
