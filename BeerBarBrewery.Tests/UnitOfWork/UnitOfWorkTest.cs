using Database.BeerBarBrewery;
using Database.BeerBarBrewery.Repository.Interface;
using Database.BeerBarBrewery.UnitOfWork;
using Database.Entities;

namespace BeerBarBrewery.Tests.UnitOfWork
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;
    using System.Threading.Tasks;

    /// <summary>
    /// Unit tests for UnitOfWork class.
    /// Tests repository coordination and transaction management.
    /// </summary>
    [TestFixture]
    public class UnitOfWorkTest
    {
        private ApplicationDbContext _context = null!;
        private Mock<IBreweryRepository> _mockBreweryRepository = null!;
        private Mock<IBeerRepository> _mockBeerRepository = null!;
        private Mock<IBarRepository> _mockBarRepository = null!;
        private Database.BeerBarBrewery.UnitOfWork.UnitOfWork _unitOfWork = null!;

        /// <summary>
        /// Initializes mocks and UnitOfWork before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UnitOfWorkTest")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _mockBreweryRepository = new Mock<IBreweryRepository>();
            _mockBeerRepository = new Mock<IBeerRepository>();
            _mockBarRepository = new Mock<IBarRepository>();

            _unitOfWork = new Database.BeerBarBrewery.UnitOfWork.UnitOfWork(
                _context,
                _mockBreweryRepository.Object,
                _mockBeerRepository.Object,
                _mockBarRepository.Object);
        }

        /// <summary>
        /// Cleans up resources after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }

        /// <summary>
        /// Verifies that repository properties are correctly initialized.
        /// </summary>
        [Test]
        public void Constructor_InitializesRepositories()
        {
            Assert.That(_unitOfWork.Breweries, Is.EqualTo(_mockBreweryRepository.Object));
            Assert.That(_unitOfWork.Beers, Is.EqualTo(_mockBeerRepository.Object));
            Assert.That(_unitOfWork.Bars, Is.EqualTo(_mockBarRepository.Object));
        }

        /// <summary>
        /// Verifies SaveChangesAsync returns true when changes exist.
        /// </summary>
        [Test]
        public async Task SaveChangesAsync_ReturnsTrue_WhenChangesExist()
        {
            _context.Beers.Add(new Beer { Name = "Test Beer", PercentageAlcoholByVolume = 5.0M });

            var result = await _unitOfWork.SaveChangesAsync();

            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Verifies SaveChangesAsync returns false when no changes exist.
        /// </summary>
        [Test]
        public async Task SaveChangesAsync_ReturnsFalse_WhenNoChanges()
        {
            var result = await _unitOfWork.SaveChangesAsync();

            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Verifies Dispose properly disposes the context.
        /// </summary>
        [Test]
        public void Dispose_DisposesContext()
        {
            _unitOfWork.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _context.Beers.ToList());
        }

        /// <summary>
        /// Verifies repositories share the same context for coordinated operations.
        /// </summary>
        [Test]
        public async Task Repositories_ShareSameContext_ForCoordinatedOperations()
        {
            var beer = new Beer { Name = "Shared Beer", PercentageAlcoholByVolume = 4.5M };
            var bar = new Bar { Name = "Shared Bar", Address = "Shared Address" };

            _context.Beers.Add(beer);
            _context.Bars.Add(bar);

            var result = await _unitOfWork.SaveChangesAsync();

            Assert.That(result, Is.True);
            Assert.That(await _context.Beers.CountAsync(), Is.EqualTo(1));
            Assert.That(await _context.Bars.CountAsync(), Is.EqualTo(1));
        }
    }
}