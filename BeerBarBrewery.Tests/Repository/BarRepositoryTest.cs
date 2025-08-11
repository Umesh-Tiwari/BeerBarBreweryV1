using Database.BeerBarBrewery;
using Database.BeerBarBrewery.Repository;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeerBarBrewery.Tests.Repository
{
    /// <summary>
    /// Unit tests for BarRepository using in-memory database.
    /// Covers CRUD operations and beer-bar relationship management.
    /// </summary>
    [TestFixture]
    public class BarRepositoryTest
    {
        private ApplicationDbContext _context;
        private BarRepository _repository;

        /// <summary>
        /// Initializes in-memory database and BarRepository before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BarDbTest")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new BarRepository(_context);
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
        /// Verifies that a bar is added to the database using AddAsync and SaveChangesAsync.
        /// </summary>
        [Test]
        public async Task AddAsync_AddsBarToDatabase()
        {
            var bar = new Bar { Name = "Test Bar", Address = "123 Test St" };

            await _repository.AddAsync(bar);
            await _repository.SaveChangesAsync();

            var bars = await _context.Bars.ToListAsync();
            Assert.That(bars.Count, Is.EqualTo(1));
            Assert.That(bars[0].Name, Is.EqualTo("Test Bar"));
        }

        /// <summary>
        /// Verifies GetAllAsync returns all bars present in the database.
        /// </summary>
        [Test]
        public async Task GetAllAsync_ReturnsAllBars()
        {
            _context.Bars.AddRange(
                new Bar { Name = "Bar A", Address = "Address A" },
                new Bar { Name = "Bar B", Address = "Address B" }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        /// <summary>
        /// Verifies GetByIdAsync returns the correct bar for a valid ID.
        /// </summary>
        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsBar()
        {
            var bar = new Bar { Name = "Bar C", Address = "Address C" };
            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(bar.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Bar C"));
        }

        /// <summary>
        /// Verifies GetByIdAsync returns null for non-existing ID.
        /// </summary>
        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Verifies Update modifies a bar's data and persists changes.
        /// </summary>
        [Test]
        public async Task Update_ModifiesBar()
        {
            var bar = new Bar { Name = "Original", Address = "Original Address" };
            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();

            bar.Name = "Updated";
            _repository.Update(bar);
            await _repository.SaveChangesAsync();

            var updatedBar = await _context.Bars.FindAsync(bar.Id);
            Assert.That(updatedBar?.Name, Is.EqualTo("Updated"));
        }

        /// <summary>
        /// Verifies Delete removes a bar from the database.
        /// </summary>
        [Test]
        public async Task Delete_RemovesBar()
        {
            var bar = new Bar { Name = "ToDelete", Address = "Delete Address" };
            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();

            _repository.Delete(bar);
            await _repository.SaveChangesAsync();

            var result = await _context.Bars.FindAsync(bar.Id);
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Verifies AssignBeerAsync creates a new bar-beer association.
        /// </summary>
        [Test]
        public async Task AssignBeerAsync_CreatesNewAssociation()
        {
            var bar = new Bar { Name = "Test Bar", Address = "Test Address" };
            var beer = new Beer { Name = "Test Beer", PercentageAlcoholByVolume = 5.0M };
            _context.Bars.Add(bar);
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            await _repository.AssignBeerAsync(bar.Id, beer.Id);
            await _repository.SaveChangesAsync();

            var association = await _context.BarBeers
                .FirstOrDefaultAsync(bb => bb.BarId == bar.Id && bb.BeerId == beer.Id);
            Assert.That(association, Is.Not.Null);
        }

        /// <summary>
        /// Verifies AssignBeerAsync doesn't create duplicate associations.
        /// </summary>
        [Test]
        public async Task AssignBeerAsync_ExistingAssociation_DoesNotCreateDuplicate()
        {
            var bar = new Bar { Name = "Test Bar", Address = "Test Address" };
            var beer = new Beer { Name = "Test Beer", PercentageAlcoholByVolume = 5.0M };
            _context.Bars.Add(bar);
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            await _repository.AssignBeerAsync(bar.Id, beer.Id);
            await _repository.SaveChangesAsync();
            await _repository.AssignBeerAsync(bar.Id, beer.Id);
            await _repository.SaveChangesAsync();

            var count = await _context.BarBeers
                .CountAsync(bb => bb.BarId == bar.Id && bb.BeerId == beer.Id);
            Assert.That(count, Is.EqualTo(1));
        }

        /// <summary>
        /// Verifies GetBeersServedAtBarAsync returns correct beers for a bar.
        /// </summary>
        [Test]
        public async Task GetBeersServedAtBarAsync_ReturnsCorrectBeers()
        {
            var bar = new Bar { Name = "Test Bar", Address = "Test Address" };
            var beer1 = new Beer { Name = "Beer 1", PercentageAlcoholByVolume = 4.5M };
            var beer2 = new Beer { Name = "Beer 2", PercentageAlcoholByVolume = 6.0M };
            _context.Bars.Add(bar);
            _context.Beers.AddRange(beer1, beer2);
            await _context.SaveChangesAsync();

            _context.BarBeers.AddRange(
                new BarBeer { BarId = bar.Id, BeerId = beer1.Id },
                new BarBeer { BarId = bar.Id, BeerId = beer2.Id }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetBeersServedAtBarAsync(bar.Id);

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(b => b.Name == "Beer 1"), Is.True);
            Assert.That(result.Any(b => b.Name == "Beer 2"), Is.True);
        }

        /// <summary>
        /// Verifies GetBeersServedAtBarAsync returns empty for bar with no beers.
        /// </summary>
        [Test]
        public async Task GetBeersServedAtBarAsync_NoBeers_ReturnsEmpty()
        {
            var bar = new Bar { Name = "Empty Bar", Address = "Empty Address" };
            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();

            var result = await _repository.GetBeersServedAtBarAsync(bar.Id);

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Verifies GetAllBarsWithBeersAsync returns bars with their associated beers.
        /// </summary>
        [Test]
        public async Task GetAllBarsWithBeersAsync_ReturnsBarsWithBeers()
        {
            var bar = new Bar { Name = "Test Bar", Address = "Test Address" };
            var beer = new Beer { Name = "Test Beer", PercentageAlcoholByVolume = 5.0M };
            _context.Bars.Add(bar);
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            _context.BarBeers.Add(new BarBeer { BarId = bar.Id, BeerId = beer.Id });
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllBarsWithBeersAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            var barWithBeers = result.First();
            Assert.That(barWithBeers.BarBeers.Count, Is.EqualTo(1));
            Assert.That(barWithBeers.BarBeers.First().Beer.Name, Is.EqualTo("Test Beer"));
        }

        /// <summary>
        /// Verifies SaveChangesAsync returns true when changes exist.
        /// </summary>
        [Test]
        public async Task SaveChangesAsync_ReturnsTrue_WhenChangesExist()
        {
            var bar = new Bar { Name = "Test Bar", Address = "Test Address" };
            await _repository.AddAsync(bar);

            var result = await _repository.SaveChangesAsync();

            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Verifies SaveChangesAsync returns false when no changes exist.
        /// </summary>
        [Test]
        public async Task SaveChangesAsync_ReturnsFalse_WhenNoChanges()
        {
            var result = await _repository.SaveChangesAsync();

            Assert.That(result, Is.False);
        }
    }
}