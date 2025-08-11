using Database.BeerBarBrewery;
using Database.BeerBarBrewery.Repository;
using Database.Entities;

namespace BeerBarBrewery.Tests.Repository
{
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Unit tests for BeerRepository using in-memory database.
    /// Covers CRUD operations and querying by alcohol volume range.
    /// </summary>
    [TestFixture]
    public class BeerRepositoryTest
    {
        private ApplicationDbContext _context;
        private BeerRepository _repository;

        /// <summary>
        /// Initializes in-memory database and BeerRepository before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BeerDbTest")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted(); // Clean slate for each test run
            _context.Database.EnsureCreated();

            _repository = new BeerRepository(_context);
        }

        /// <summary>
        /// Cleans up database context after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        /// <summary>
        /// Verifies that a beer is added to the database using AddAsync and SaveChangesAsync.
        /// </summary>
        [Test]
        public async Task AddAsync_AddsBeerToDatabase()
        {
            var beer = new Beer { Name = "Test Beer", PercentageAlcoholByVolume = 5.5M };

            await _repository.AddAsync(beer);
            await _repository.SaveChangesAsync();

            var beers = await _context.Beers.ToListAsync();
            Assert.That(beers.Count, Is.EqualTo(1));
            Assert.That(beers[0].Name, Is.EqualTo("Test Beer"));
        }

        /// <summary>
        /// Verifies GetAllAsync returns all beers present in the database.
        /// </summary>
        [Test]
        public async Task GetAllAsync_ReturnsAllBeers()
        {
            _context.Beers.AddRange(
                new Beer { Name = "Beer A", PercentageAlcoholByVolume = 4.5M },
                new Beer { Name = "Beer B", PercentageAlcoholByVolume = 6.0M }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        /// <summary>
        /// Verifies GetByIdAsync returns the correct beer for a valid ID.
        /// </summary>
        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsBeer()
        {
            var beer = new Beer { Name = "Beer C", PercentageAlcoholByVolume = 7.0M };
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(beer.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Beer C"));
        }

        /// <summary>
        /// Verifies beers are filtered correctly by alcohol volume range.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRangeAsync_ReturnsCorrectBeers()
        {
            _context.Beers.AddRange(
                new Beer { Name = "Low ABV", PercentageAlcoholByVolume = 3.5M },
                new Beer { Name = "Mid ABV", PercentageAlcoholByVolume = 5.0M },
                new Beer { Name = "High ABV", PercentageAlcoholByVolume = 8.0M }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetBeersByAlcoholVolumeRangeAsync(4.0, 6.0);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Mid ABV"));
        }

        /// <summary>
        /// Verifies Update modifies a beer's data and persists changes.
        /// </summary>
        [Test]
        public async Task Update_ModifiesBeer()
        {
            var beer = new Beer { Name = "Original", PercentageAlcoholByVolume = 4.0M };
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            beer.Name = "Updated";
            _repository.Update(beer);
            await _repository.SaveChangesAsync();

            var updatedBeer = await _context.Beers.FindAsync(beer.Id);
            Assert.That(updatedBeer?.Name, Is.EqualTo("Updated"));
        }

        /// <summary>
        /// Verifies Delete removes a beer from the database.
        /// </summary>
        [Test]
        public async Task Delete_RemovesBeer()
        {
            var beer = new Beer { Name = "ToDelete", PercentageAlcoholByVolume = 5.0M };
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            _repository.Delete(beer);
            await _repository.SaveChangesAsync();

            var result = await _context.Beers.FindAsync(beer.Id);
            Assert.That(result, Is.Null);
        }
    }
}
