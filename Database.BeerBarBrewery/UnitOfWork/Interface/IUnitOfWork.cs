using Database.BeerBarBrewery.Repository.Interface;

namespace Database.BeerBarBrewery.UnitOfWork.Interface
{
    /// <summary>
    /// Unit of Work interface for coordinating multiple repository operations with a single database context.
    /// Ensures atomic commits and proper resource disposal.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the Beer repository.
        /// </summary>
        IBeerRepository Beers { get; }

        /// <summary>
        /// Gets the Brewery repository.
        /// </summary>
        IBreweryRepository Breweries { get; }

        /// <summary>
        /// Gets the Bar repository.
        /// </summary>
        IBarRepository Bars { get; }

        /// <summary>
        /// Commits all changes made through the repositories to the database.
        /// </summary>
        /// <returns>True if changes were saved successfully; otherwise, false.</returns>
        Task<bool> SaveChangesAsync();
    }

}
