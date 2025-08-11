using Database.BeerBarBrewery;
using Database.BeerBarBrewery.Repository;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeerBarBrewery.Tests.Repository
{
    /// <summary>
    /// Unit tests for BreweryRepository using in-memory database.
    /// Covers CRUD operations and querying with beer relationships.
    /// </summary>
    [TestFixture]
    public class BreweryRepositoryTests
    {
        private ApplicationDbContext _context;
        private BreweryRepository _repository;

        /// <summary>
        /// Initializes in-memory database and BreweryRepository before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BreweryDbTest")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new BreweryRepository(_context);
        }

        /// <summary>
        /// Cleans up database context after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region AddAsync Tests

        [Test]
        public async Task AddAsync_AddsBreweryToDatabase()
        {
            var brewery = new Brewery { Name = "Test Brewery" };

            await _repository.AddAsync(brewery);
            await _repository.SaveChangesAsync();

            var breweries = await _context.Breweries.ToListAsync();
            Assert.That(breweries.Count, Is.EqualTo(1));
            Assert.That(breweries[0].Name, Is.EqualTo("Test Brewery"));
        }

        #endregion

        #region GetAllAsync Tests

        [Test]
        public async Task GetAllAsync_ReturnsAllBreweries()
        {
            _context.Breweries.AddRange(
                new Brewery { Name = "Brewery A" },
                new Brewery { Name = "Brewery B" }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        #endregion

        #region GetAllWithBeerAsync Tests

        [Test]
        public async Task GetAllWithBeerAsync_ReturnsBreweriesWithBeers()
        {
            var brewery = new Brewery { Name = "Test Brewery",BreweryBeers = new List<BreweryBeer> { new BreweryBeer { BeerId = 1, BreweryId=1 } } };
            _context.Breweries.Add(brewery);
            await _context.SaveChangesAsync();

            var beer = new Beer { Name = "Test Beer" };
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllWithBeerAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().BreweryBeers.Count, Is.EqualTo(1));
        }

        #endregion

        #region GetByIdAsync Tests

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsBreweryWithBeers()
        {
            var brewery = new Brewery { Name = "Test Brewery" };
            _context.Breweries.Add(brewery);
            await _context.SaveChangesAsync();

            var beer = new Beer { Name = "Test Beer" };
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(brewery.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Brewery"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        #endregion

        #region Update Tests

        [Test]
        public async Task Update_ModifiesBrewery()
        {
            var brewery = new Brewery { Name = "Original Brewery" };
            _context.Breweries.Add(brewery);
            await _context.SaveChangesAsync();

            brewery.Name = "Updated Brewery";
            _repository.Update(brewery);
            await _repository.SaveChangesAsync();

            var updatedBrewery = await _context.Breweries.FindAsync(brewery.Id);
            Assert.That(updatedBrewery?.Name, Is.EqualTo("Updated Brewery"));
        }

        #endregion

        #region Delete Tests

        [Test]
        public async Task Delete_RemovesBrewery()
        {
            var brewery = new Brewery { Name = "ToDelete Brewery" };
            _context.Breweries.Add(brewery);
            await _context.SaveChangesAsync();

            _repository.Delete(brewery);
            await _repository.SaveChangesAsync();

            var result = await _context.Breweries.FindAsync(brewery.Id);
            Assert.That(result, Is.Null);
        }

        #endregion

        #region SaveChangesAsync Tests

        [Test]
        public async Task SaveChangesAsync_ReturnsTrue_WhenChangesExist()
        {
            var brewery = new Brewery { Name = "Test Brewery" };
            await _repository.AddAsync(brewery);

            var result = await _repository.SaveChangesAsync();

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task SaveChangesAsync_ReturnsFalse_WhenNoChanges()
        {
            var result = await _repository.SaveChangesAsync();

            Assert.That(result, Is.False);
        }

        #endregion
    }
}