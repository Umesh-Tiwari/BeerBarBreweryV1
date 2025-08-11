using AutoMapper;
using Business.BeerBarBrewery.Process;
using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Model.BeerBarBrewery;
using Moq;
using NUnit.Framework;

namespace BeerBarBrewery.Tests.BusinessProcess
{
    /// <summary>
    /// Unit tests for BreweryProcess business logic layer.
    /// </summary>
    [TestFixture]
    public class BreweryProcessTests
    {
        private Mock<IBreweryRepository> _mockBreweryRepository = null!;
        private Mock<IBeerRepository> _mockBeerRepository = null!;
        private Mock<IMapper> _mockMapper = null!;
        private BreweryProcess _breweryProcess = null!;

        [SetUp]
        public void SetUp()
        {
            _mockBreweryRepository = new Mock<IBreweryRepository>();
            _mockBeerRepository = new Mock<IBeerRepository>();
            _mockMapper = new Mock<IMapper>();
            _breweryProcess = new BreweryProcess(_mockBreweryRepository.Object, _mockBeerRepository.Object, _mockMapper.Object);
        }

        #region GetAllBreweries Tests

        [Test]
        public async Task GetAllBreweries_ReturnsBreweries_WhenBreweriesExist()
        {
            var breweryEntities = new List<Brewery> { new Brewery { Id = 1, Name = "Test Brewery" } };
            var breweryModels = new List<BreweryModel> { new BreweryModel { Id = 1, Name = "Test Brewery" } };

            _mockBreweryRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(breweryEntities);
            _mockMapper.Setup(x => x.Map<IEnumerable<BreweryModel>>(breweryEntities)).Returns(breweryModels);

            var result = await _breweryProcess.GetAllBreweries();

            Assert.That(result, Is.EqualTo(breweryModels));
        }

        [Test]
        public async Task GetAllBreweries_ReturnsEmpty_WhenNoBreweriesExist()
        {
            _mockBreweryRepository.Setup(x => x.GetAllAsync()).ReturnsAsync((IEnumerable<Brewery>)null);

            var result = await _breweryProcess.GetAllBreweries();

            Assert.That(result, Is.Empty);
        }

        #endregion

        #region GetAllWithBeerAsync Tests

        [Test]
        public async Task GetAllWithBeerAsync_ReturnsBreweriesWithBeers_WhenBreweriesExist()
        {
            var breweryEntities = new List<Brewery> { new Brewery { Id = 1, Name = "Test Brewery" } };
            var breweryModels = new List<BreweryModel> { new BreweryModel { Id = 1, Name = "Test Brewery" } };

            _mockBreweryRepository.Setup(x => x.GetAllWithBeerAsync()).ReturnsAsync(breweryEntities);
            _mockMapper.Setup(x => x.Map<IEnumerable<BreweryModel>>(breweryEntities)).Returns(breweryModels);

            var result = await _breweryProcess.GetAllWithBeerAsync();

            Assert.That(result, Is.EqualTo(breweryModels));
        }

        [Test]
        public async Task GetAllWithBeerAsync_ReturnsEmpty_WhenNoBreweriesExist()
        {
            _mockBreweryRepository.Setup(x => x.GetAllWithBeerAsync()).ReturnsAsync((IEnumerable<Brewery>)null);

            var result = await _breweryProcess.GetAllWithBeerAsync();

            Assert.That(result, Is.Empty);
        }

        #endregion

        #region GetBreweryById Tests

        [Test]
        public async Task GetBreweryById_ReturnsBrewery_WhenBreweryExists()
        {
            var breweryEntity = new Brewery { Id = 1, Name = "Test Brewery" };
            var breweryModel = new BreweryModel { Id = 1, Name = "Test Brewery" };

            _mockBreweryRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(breweryEntity);
            _mockMapper.Setup(x => x.Map<BreweryModel>(breweryEntity)).Returns(breweryModel);

            var result = await _breweryProcess.GetBreweryById(1);

            Assert.That(result, Is.EqualTo(breweryModel));
        }

        [Test]
        public async Task GetBreweryById_ReturnsNull_WhenBreweryNotFound()
        {
            _mockBreweryRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Brewery)null);

            var result = await _breweryProcess.GetBreweryById(999);

            Assert.That(result, Is.Null);
        }

        #endregion

        #region CreateBrewery Tests

        [Test]
        public async Task CreateBrewery_ReturnsCreatedBrewery()
        {
            var createBreweryModel = new CreateBreweryModel { Name = "New Brewery" };
            var breweryEntity = new Brewery { Id = 1, Name = "New Brewery" };
            var breweryModel = new BreweryModel { Id = 1, Name = "New Brewery" };

            _mockMapper.Setup(x => x.Map<Brewery>(createBreweryModel)).Returns(breweryEntity);
            _mockBreweryRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<BreweryModel>(breweryEntity)).Returns(breweryModel);

            var result = await _breweryProcess.CreateBrewery(createBreweryModel);

            Assert.That(result, Is.EqualTo(breweryModel));
            _mockBreweryRepository.Verify(x => x.AddAsync(breweryEntity), Times.Once);
            _mockBreweryRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateBrewery Tests

        [Test]
        public async Task UpdateBrewery_ReturnsTrue_WhenBreweryExists()
        {
            var breweryEntity = new Brewery { Id = 1, Name = "Old Brewery" };
            var updateModel = new CreateBreweryModel { Name = "Updated Brewery" };

            _mockBreweryRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(breweryEntity);
            _mockBreweryRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _breweryProcess.UpdateBrewery(1, updateModel);

            Assert.That(result, Is.True);
            _mockMapper.Verify(x => x.Map(updateModel, breweryEntity), Times.Once);
            _mockBreweryRepository.Verify(x => x.Update(breweryEntity), Times.Once);
            _mockBreweryRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateBrewery_ReturnsFalse_WhenBreweryNotFound()
        {
            var updateModel = new CreateBreweryModel { Name = "Updated Brewery" };

            _mockBreweryRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Brewery)null);

            var result = await _breweryProcess.UpdateBrewery(999, updateModel);

            Assert.That(result, Is.False);
        }

        #endregion

        #region AssignBreweryToBeer Tests

        [Test]
        public async Task AssignBreweryToBeer_ReturnsTrue_WhenBothExist()
        {
            var breweryEntity = new Brewery { Id = 1, Name = "Test Brewery" };
            var beerEntity = new Beer { Id = 1, Name = "Test Beer" };
            var breweryBeerModel = new BreweryBeerModel { BreweryId = 1, BeerId = 1 };

            _mockBreweryRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(breweryEntity);
            _mockBeerRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(beerEntity);
            _mockBeerRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _breweryProcess.AssignBreweryToBeer(breweryBeerModel);

            Assert.That(result, Is.True);
            Assert.That(beerEntity.BreweryId, Is.EqualTo(1));
            _mockBeerRepository.Verify(x => x.Update(beerEntity), Times.Once);
            _mockBeerRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AssignBreweryToBeer_ReturnsFalse_WhenBreweryNotFound()
        {
            var beerEntity = new Beer { Id = 1, Name = "Test Beer" };
            var breweryBeerModel = new BreweryBeerModel { BreweryId = 999, BeerId = 1 };

            _mockBreweryRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Brewery)null);
            _mockBeerRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(beerEntity);

            var result = await _breweryProcess.AssignBreweryToBeer(breweryBeerModel);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AssignBreweryToBeer_ReturnsFalse_WhenBeerNotFound()
        {
            var breweryEntity = new Brewery { Id = 1, Name = "Test Brewery" };
            var breweryBeerModel = new BreweryBeerModel { BreweryId = 1, BeerId = 999 };

            _mockBreweryRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(breweryEntity);
            _mockBeerRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Beer)null);

            var result = await _breweryProcess.AssignBreweryToBeer(breweryBeerModel);

            Assert.That(result, Is.False);
        }

        #endregion

        #region DeleteBrewery Tests

        [Test]
        public async Task DeleteBrewery_ReturnsTrue_WhenBreweryExists()
        {
            var breweryEntity = new Brewery { Id = 1, Name = "Test Brewery" };

            _mockBreweryRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(breweryEntity);
            _mockBreweryRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _breweryProcess.DeleteBrewery(1);

            Assert.That(result, Is.True);
            _mockBreweryRepository.Verify(x => x.Delete(breweryEntity), Times.Once);
            _mockBreweryRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteBrewery_ReturnsFalse_WhenBreweryNotFound()
        {
            _mockBreweryRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Brewery)null);

            var result = await _breweryProcess.DeleteBrewery(999);

            Assert.That(result, Is.False);
        }

        #endregion
    }
}