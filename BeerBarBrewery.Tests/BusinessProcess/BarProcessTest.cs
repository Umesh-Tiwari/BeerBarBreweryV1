using AutoMapper;
using Business.BeerBarBrewery.Process;
using Business.BeerBarBrewery.Process.Interface;
using Contract.BeerBarBrewery;
using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Model.BeerBarBrewery;
using Moq;

namespace BeerBarBrewery.Tests.BusinessProcess
{
    /// <summary>
    /// Unit tests for BarProcess class which handles business logic for bars.
    /// Tests include CRUD operations and linking beers to bars.
    /// </summary>
    public class BarProcessTest
    {
        private readonly Mock<IBarRepository> _mockBarRepository;
        private readonly Mock<IBeerRepository> _mockBeerRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IBarProcess _barProcess;

        /// <summary>
        /// Initializes mock dependencies and BarProcess instance for testing.
        /// </summary>
        public BarProcessTest()
        {
            _mockBarRepository = new Mock<IBarRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockBeerRepository = new Mock<IBeerRepository>();
            _barProcess = new BarProcess(_mockBarRepository.Object, _mockBeerRepository.Object, _mockMapper.Object);
        }

        /// <summary>
        /// Verifies GetAllBars fetches all bars and maps them correctly to BarModel.
        /// </summary>
        [Test]
        public async Task GetAllBars_ReturnsMappedBarModels()
        {
            var barEntities = new List<Bar> { new Bar { Id = 1 }, new Bar { Id = 2 } };
            var barModels = new List<BarModel> { new BarModel { Id = 1 }, new BarModel { Id = 2 } };

            _mockBarRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(barEntities);
            _mockMapper.Setup(m => m.Map<IEnumerable<BarModel>>(barEntities)).Returns(barModels);

            var result = await _barProcess.GetAllBars();

            Assert.That(result, Is.EqualTo(barModels));
            _mockBarRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        /// <summary>
        /// Verifies GetAllBarsWithBeers fetches bars with their beers and maps to BarModel.
        /// </summary>
        [Test]
        public async Task GetAllBarsWithBeers_ReturnsMappedBarModels()
        {
            var barEntities = new List<Bar> { new Bar { Id = 1 }, new Bar { Id = 2 } };
            var barModels = new List<BarModel> { new BarModel { Id = 1 }, new BarModel { Id = 2 } };

            _mockBarRepository.Setup(r => r.GetAllBarsWithBeersAsync()).ReturnsAsync(barEntities);
            _mockMapper.Setup(m => m.Map<IEnumerable<BarModel>>(barEntities)).Returns(barModels);

            var result = await _barProcess.GetAllBarsWithBeers();

            Assert.That(result, Is.EqualTo(barModels));
            _mockBarRepository.Verify(r => r.GetAllBarsWithBeersAsync(), Times.Once);
        }

        /// <summary>
        /// Verifies CreateBar adds a new bar and returns the created BarModel.
        /// </summary>
        [Test]
        public async Task CreateBarAsync_ValidModel_AddsAndReturnsBarModel()
        {
            var model = new CreateBarModel { Name = "New Bar" };
            var barEntity = new Bar { Id = 1, Name = "New Bar" };
            var returnedModel = new BarModel { Id = 1, Name = "New Bar" };

            _mockMapper.Setup(m => m.Map<Bar>(model)).Returns(barEntity);
            _mockBarRepository.Setup(r => r.AddAsync(barEntity)).Returns(Task.CompletedTask);
            _mockBarRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<BarModel>(barEntity)).Returns(returnedModel);

            var result = await _barProcess.CreateBar(model);

            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("New Bar"));
        }

        /// <summary>
        /// Verifies UpdateBar updates an existing bar and returns true.
        /// </summary>
        [Test]
        public async Task UpdateBarAsync_BarExists_UpdatesAndReturnsTrue()
        {
            int barId = 1;
            var model = new CreateBarModel { Name = "Updated Bar" };
            var barEntity = new Bar { Id = barId, Name = "Old Bar" };

            _mockBarRepository.Setup(r => r.GetByIdAsync(barId)).ReturnsAsync(barEntity);
            _mockMapper.Setup(m => m.Map(model, barEntity)).Verifiable();
            _mockBarRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _barProcess.UpdateBar(barId, model);

            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Verifies UpdateBar returns false if bar does not exist.
        /// </summary>
        [Test]
        public async Task UpdateBarAsync_BarDoesNotExist_ReturnsFalse()
        {
            int barId = 999;
            var model = new CreateBarModel { Name = "Nonexistent" };

            _mockBarRepository.Setup(r => r.GetByIdAsync(barId)).ReturnsAsync((Bar)null);

            var result = await _barProcess.UpdateBar(barId, model);

            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Verifies DeleteBar removes an existing bar and returns true.
        /// </summary>
        [Test]
        public async Task DeleteBarAsync_BarExists_DeletesAndReturnsTrue()
        {
            int barId = 1;
            var barEntity = new Bar { Id = barId };

            _mockBarRepository.Setup(r => r.GetByIdAsync(barId)).ReturnsAsync(barEntity);
            _mockBarRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _barProcess.DeleteBar(barId);

            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Verifies DeleteBar returns false when the bar does not exist.
        /// </summary>
        [Test]
        public async Task DeleteBarAsync_BarDoesNotExist_ReturnsFalse()
        {
            int barId = 999;

            _mockBarRepository.Setup(r => r.GetByIdAsync(barId)).ReturnsAsync((Bar)null);

            var result = await _barProcess.DeleteBar(barId);

            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Verifies linking an existing beer to an existing bar returns true.
        /// </summary>
        [Test]
        public async Task LinkBarToBeer_BarAndBeerExist_ReturnsTrue()
        {
            var dto = new BarBeerRequest { BarId = 1, BeerId = 2 };
            var bar = new Bar { Id = 1 };
            var beer = new Beer { Id = 2 };

            _mockBarRepository.Setup(r => r.GetByIdAsync(dto.BarId)).ReturnsAsync(bar);
            _mockBeerRepository.Setup(r => r.GetByIdAsync(dto.BeerId)).ReturnsAsync(beer);
            _mockBarRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _barProcess.LinkBarToBeer(dto);

            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Verifies linking fails and returns false if the bar is not found.
        /// </summary>
        [Test]
        public async Task LinkBarToBeer_BarNotFound_ReturnsFalse()
        {
            var dto = new BarBeerRequest { BarId = 1, BeerId = 2 };
            var beer = new Beer { Id = 2 };

            _mockBarRepository.Setup(r => r.GetByIdAsync(dto.BarId)).ReturnsAsync((Bar)null);
            _mockBeerRepository.Setup(r => r.GetByIdAsync(dto.BeerId)).ReturnsAsync(beer);

            var result = await _barProcess.LinkBarToBeer(dto);

            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Verifies linking fails and returns false if the beer is not found.
        /// </summary>
        [Test]
        public async Task LinkBarToBeer_BeerNotFound_ReturnsFalse()
        {
            var dto = new BarBeerRequest { BarId = 1, BeerId = 2 };
            var bar = new Bar { Id = 1 };

            _mockBarRepository.Setup(r => r.GetByIdAsync(dto.BarId)).ReturnsAsync(bar);
            _mockBeerRepository.Setup(r => r.GetByIdAsync(dto.BeerId)).ReturnsAsync((Beer)null);

            var result = await _barProcess.LinkBarToBeer(dto);

            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Verifies getting beers served at a bar returns mapped BeerModel list.
        /// </summary>
        [Test]
        public async Task GetBeersServedAtBarAsync_BeersExist_ReturnsMappedBeerModels()
        {
            int barId = 1;
            var beerEntities = new List<Beer> { new Beer { Id = 1 }, new Beer { Id = 2 } };
            var beerModels = new List<BeerModel> { new BeerModel { Id = 1 }, new BeerModel { Id = 2 } };

            _mockBarRepository.Setup(r => r.GetBeersServedAtBarAsync(barId)).ReturnsAsync(beerEntities);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeerModel>>(beerEntities)).Returns(beerModels);

            var result = await _barProcess.GetBeersServedAtBarAsync(barId);

            Assert.That(result, Is.EqualTo(beerModels));
        }

        /// <summary>
        /// Verifies getting beers at a bar returns empty list if no beers found.
        /// </summary>
        [Test]
        public async Task GetBeersServedAtBarAsync_NoBeers_ReturnsEmptyList()
        {
            int barId = 2;
            var emptyBeerList = new List<Beer>();
            var emptyBeerModelList = new List<BeerModel>();

            _mockBarRepository.Setup(r => r.GetBeersServedAtBarAsync(barId)).ReturnsAsync(emptyBeerList);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeerModel>>(emptyBeerList)).Returns(emptyBeerModelList);

            var result = await _barProcess.GetBeersServedAtBarAsync(barId);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verifies getting beers at a bar returns empty list if repository returns null.
        /// </summary>
        [Test]
        public async Task GetBeersServedAtBarAsync_NullBeerList_ReturnsEmptyList()
        {
            int barId = 3;
            _mockBarRepository.Setup(r => r.GetBeersServedAtBarAsync(barId)).ReturnsAsync((List<Beer>)null);

            var result = await _barProcess.GetBeersServedAtBarAsync(barId);

            Assert.That(result, Is.Empty);
        }
    }
}
