using AutoMapper;
using Business.BeerBarBrewery.Process;
using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Model.BeerBarBrewery;
using Moq;

namespace BeerBarBrewery.Tests.BusinessProcess
{
    /// <summary>
    /// Unit tests for BeerProcess business logic layer.
    /// </summary>
    [TestFixture]
    public class BeerProcessTests
    {
        private Mock<IBeerRepository> _mockBeerRepository = null!;
        private Mock<IMapper> _mockMapper = null!;
        private BeerProcess _beerProcess = null!;

        [SetUp]
        public void SetUp()
        {
            _mockBeerRepository = new Mock<IBeerRepository>();
            _mockMapper = new Mock<IMapper>();
            _beerProcess = new BeerProcess(_mockBeerRepository.Object, _mockMapper.Object);
        }

        #region GetBeerById Tests

        [Test]
        public async Task GetBeerById_ReturnsBeer_WhenBeerExists()
        {
            var beerEntity = new Beer { Id = 1, Name = "Test Beer" };
            var beerModel = new BeerModel { Id = 1, Name = "Test Beer" };

            _mockBeerRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(beerEntity);
            _mockMapper.Setup(x => x.Map<BeerModel>(beerEntity)).Returns(beerModel);

            var result = await _beerProcess.GetBeerById(1);

            Assert.That(result, Is.EqualTo(beerModel));
        }

        [Test]
        public async Task GetBeerById_ReturnsNull_WhenBeerNotFound()
        {
            _mockBeerRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Beer)null);

            var result = await _beerProcess.GetBeerById(999);

            Assert.That(result, Is.Null);
        }

        #endregion

        #region CreateBeer Tests

        [Test]
        public async Task CreateBeer_ReturnsCreatedBeer()
        {
            var createBeerModel = new CreateBeerModel { Name = "New Beer" };
            var beerEntity = new Beer { Id = 1, Name = "New Beer" };
            var beerModel = new BeerModel { Id = 1, Name = "New Beer" };

            _mockMapper.Setup(x => x.Map<Beer>(createBeerModel)).Returns(beerEntity);
            _mockBeerRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<BeerModel>(beerEntity)).Returns(beerModel);

            var result = await _beerProcess.CreateBeer(createBeerModel);

            Assert.That(result, Is.EqualTo(beerModel));
            _mockBeerRepository.Verify(x => x.AddAsync(beerEntity), Times.Once);
            _mockBeerRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateBeer Tests

        [Test]
        public async Task UpdateBeer_ReturnsTrue_WhenBeerExists()
        {
            var beerEntity = new Beer { Id = 1, Name = "Old Beer" };
            var updateModel = new CreateBeerModel { Name = "Updated Beer" };

            _mockBeerRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(beerEntity);
            _mockBeerRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _beerProcess.UpdateBeer(1, updateModel);

            Assert.That(result, Is.True);
            _mockMapper.Verify(x => x.Map(updateModel, beerEntity), Times.Once);
            _mockBeerRepository.Verify(x => x.Update(beerEntity), Times.Once);
            _mockBeerRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateBeer_ReturnsFalse_WhenBeerNotFound()
        {
            var updateModel = new CreateBeerModel { Name = "Updated Beer" };

            _mockBeerRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Beer)null);

            var result = await _beerProcess.UpdateBeer(999, updateModel);

            Assert.That(result, Is.False);
        }

        #endregion

        #region GetBeersByAlcoholVolumeRange Tests

        [Test]
        public async Task GetBeersByAlcoholVolumeRange_BothParameters_ReturnsBeers()
        {
            var beerEntities = new List<Beer> { new Beer { Id = 1, Name = "Test Beer" } };
            var beerModels = new List<BeerModel> { new BeerModel { Id = 1, Name = "Test Beer" } };

            _mockBeerRepository.Setup(x => x.GetBeersByAlcoholVolumeRangeAsync(3.0M, 5.0M)).ReturnsAsync(beerEntities);
            _mockMapper.Setup(x => x.Map<IEnumerable<BeerModel>>(beerEntities)).Returns(beerModels);

            var result = await _beerProcess.GetBeersByAlcoholVolumeRange(3.0M, 5.0M);

            Assert.That(result, Is.EqualTo(beerModels));
        }

        [Test]
        public async Task GetBeersByAlcoholVolumeRange_MinimumOnly_ReturnsBeers()
        {
            var beerEntities = new List<Beer> { new Beer { Id = 1, Name = "High ABV Beer" } };
            var beerModels = new List<BeerModel> { new BeerModel { Id = 1, Name = "High ABV Beer" } };

            _mockBeerRepository.Setup(x => x.GetBeersByAlcoholVolumeRangeAsync(5.0M, null)).ReturnsAsync(beerEntities);
            _mockMapper.Setup(x => x.Map<IEnumerable<BeerModel>>(beerEntities)).Returns(beerModels);

            var result = await _beerProcess.GetBeersByAlcoholVolumeRange(5.0M, null);

            Assert.That(result, Is.EqualTo(beerModels));
        }

        [Test]
        public async Task GetBeersByAlcoholVolumeRange_MaximumOnly_ReturnsBeers()
        {
            var beerEntities = new List<Beer> { new Beer { Id = 1, Name = "Low ABV Beer" } };
            var beerModels = new List<BeerModel> { new BeerModel { Id = 1, Name = "Low ABV Beer" } };

            _mockBeerRepository.Setup(x => x.GetBeersByAlcoholVolumeRangeAsync(null, 4.0M)).ReturnsAsync(beerEntities);
            _mockMapper.Setup(x => x.Map<IEnumerable<BeerModel>>(beerEntities)).Returns(beerModels);

            var result = await _beerProcess.GetBeersByAlcoholVolumeRange(null, 4.0M);

            Assert.That(result, Is.EqualTo(beerModels));
        }

        [Test]
        public async Task GetBeersByAlcoholVolumeRange_ReturnsEmpty_WhenNoBeersFound()
        {
            _mockBeerRepository.Setup(x => x.GetBeersByAlcoholVolumeRangeAsync(10.0M, 15.0M)).ReturnsAsync((IEnumerable<Beer>)null);

            var result = await _beerProcess.GetBeersByAlcoholVolumeRange(10.0M, 15.0M);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetBeersByAlcoholVolumeRange_BothNull_ReturnsEmpty()
        {
            _mockBeerRepository.Setup(x => x.GetBeersByAlcoholVolumeRangeAsync(null, null)).ReturnsAsync(new List<Beer>());

            var result = await _beerProcess.GetBeersByAlcoholVolumeRange(null, null);

            Assert.That(result, Is.Empty);
        }

        #endregion

        #region DeleteBeer Tests

        [Test]
        public async Task DeleteBeer_ReturnsTrue_WhenBeerExists()
        {
            var beerEntity = new Beer { Id = 11, Name = "Test Beer" };

            _mockBeerRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(beerEntity);
            _mockBeerRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _beerProcess.DeleteBeer(1);

            Assert.That(result, Is.True);
            _mockBeerRepository.Verify(x => x.Delete(beerEntity), Times.Once);
            _mockBeerRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteBeer_ReturnsFalse_WhenBeerNotFound()
        {
            _mockBeerRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Beer)null);

            var result = await _beerProcess.DeleteBeer(999);

            Assert.That(result, Is.False);
        }

        #endregion
    }
}