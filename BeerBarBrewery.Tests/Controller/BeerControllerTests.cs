using AutoMapper;
using BeerBarBrewery.Controllers;
using Business.BeerBarBrewery.Process.Interface;
using Contract.BeerBarBrewery;
using Microsoft.AspNetCore.Mvc;
using Model.BeerBarBrewery;
using Moq;

namespace BeerBarBrewery.Tests.Controller
{
    /// <summary>
    /// Unit tests for the BeerController, verifying controller actions with mocked dependencies.
    /// Uses NUnit, Moq, and AutoMapper.
    /// </summary>
    public class BeerControllerTests
    {
        private readonly Mock<IBeerProcess> _mockBeerProcess;
        private readonly BeerController _controller;
        private readonly Mock<IMapper> _mockMapper;

        /// <summary>
        /// Initializes mocks and the controller for testing.
        /// </summary>
        public BeerControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockBeerProcess = new Mock<IBeerProcess>();
            _controller = new BeerController(_mockBeerProcess.Object, _mockMapper.Object);
        }

        #region GetAllBeers Tests

        /// <summary>
        /// Tests that GetAllBeers returns OkObjectResult with a list of BeerResponse when beers exist.
        /// </summary>
        [Test]
        public async Task GetAllBeers_ReturnsOkWithBeerResponses()
        {
            var beerModels = new List<BeerModel>
            {
                new BeerModel { Id = 1, Name = "Beer One", PercentageAlcoholByVolume = 5.0 },
                new BeerModel { Id = 2, Name = "Beer Two", PercentageAlcoholByVolume = 4.5 }
            };

            var beerResponses = new List<BeerResponse>
            {
                new BeerResponse { Id = 1, Name = "Beer One", PercentageAlcoholByVolume = 5.0 },
                new BeerResponse { Id = 2, Name = "Beer Two", PercentageAlcoholByVolume = 4.5 }
            };

            _mockBeerProcess.Setup(x => x.GetAllBeers()).ReturnsAsync(beerModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeerResponse>>(beerModels)).Returns(beerResponses);

            var result = await _controller.GetAllBeers();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(beerResponses));
        }

        /// <summary>
        /// Tests that GetAllBeers returns NotFound when no beers exist.
        /// </summary>
        [Test]
        public async Task GetAllBeers_NoBeers_ReturnsNotFound()
        {
            var emptyBeerList = new List<BeerModel>();

            _mockBeerProcess.Setup(x => x.GetAllBeers()).ReturnsAsync(emptyBeerList);

            var result = await _controller.GetAllBeers();

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region GetBeerById Tests

        /// <summary>
        /// Tests GetBeerById returns a mapped BeerResponse when beer exists.
        /// </summary>
        [Test]
        public async Task GetBeerById_ValidId_ReturnsMappedBeerResponse()
        {
            var beerModel = new BeerModel { Id = 1, Name = "Test Beer", PercentageAlcoholByVolume = 5.0 };
            var beerResponse = new BeerResponse { Id = 1, Name = "Test Beer", PercentageAlcoholByVolume = 5.0 };

            _mockBeerProcess.Setup(x => x.GetBeerById(1)).ReturnsAsync(beerModel);
            _mockMapper.Setup(m => m.Map<BeerResponse>(beerModel)).Returns(beerResponse);

            var result = await _controller.GetBeerById(1);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(beerResponse));
        }

        /// <summary>
        /// Tests GetBeerById returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task GetBeerById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.GetBeerById(0);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeerById returns NotFound when beer doesn't exist.
        /// </summary>
        [Test]
        public async Task GetBeerById_BeerNotFound_ReturnsNotFound()
        {
            _mockBeerProcess.Setup(x => x.GetBeerById(999)).ReturnsAsync((BeerModel)null);

            var result = await _controller.GetBeerById(999);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region GetBeersByAlcoholVolumeRange Tests

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns beers within valid range.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_ValidRange_ReturnsBeers()
        {
            var beerModels = new List<BeerModel>
            {
                new BeerModel { Id = 1, Name = "Light Beer", PercentageAlcoholByVolume = 4.0 }
            };

            var beerResponses = new List<BeerResponse>
            {
                new BeerResponse { Id = 1, Name = "Light Beer", PercentageAlcoholByVolume = 4.0 }
            };

            _mockBeerProcess.Setup(x => x.GetBeersByAlcoholVolumeRange(3.0, 5.0)).ReturnsAsync(beerModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeerResponse>>(beerModels)).Returns(beerResponses);

            var result = await _controller.GetBeersByAlcoholVolumeRange(3.0, 5.0);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(beerResponses));
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns BadRequest for negative values.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_NegativeValues_ReturnsBadRequest()
        {
            var result = await _controller.GetBeersByAlcoholVolumeRange(-1.0, 5.0);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns BadRequest when min >= max.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_InvalidRange_ReturnsBadRequest()
        {
            var result = await _controller.GetBeersByAlcoholVolumeRange(5.0, 3.0);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns NotFound when no beers in range.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_NoBeersInRange_ReturnsNotFound()
        {
            var emptyBeerList = new List<BeerModel>();

            _mockBeerProcess.Setup(x => x.GetBeersByAlcoholVolumeRange(10.0, 15.0)).ReturnsAsync(emptyBeerList);

            var result = await _controller.GetBeersByAlcoholVolumeRange(10.0, 15.0);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region CreateBeer Tests

        /// <summary>
        /// Tests creating a beer returns CreatedAtActionResult with the new BeerResponse.
        /// </summary>
        [Test]
        public async Task CreateBeer_ValidRequest_ReturnsCreatedAtAction()
        {
            var createBeerRequest = new CreateBeerRequest { Name = "New Beer", PercentageAlcoholByVolume = 5.0 };
            var createBeerModel = new CreateBeerModel { Name = "New Beer", PercentageAlcoholByVolume = 5.0 };
            var beerModel = new BeerModel { Id = 1, Name = "New Beer", PercentageAlcoholByVolume = 5.0 };
            var beerResponse = new BeerResponse { Id = 1, Name = "New Beer", PercentageAlcoholByVolume = 5.0 };

            _mockMapper.Setup(m => m.Map<CreateBeerModel>(createBeerRequest)).Returns(createBeerModel);
            _mockBeerProcess.Setup(x => x.CreateBeer(createBeerModel)).ReturnsAsync(beerModel);
            _mockMapper.Setup(m => m.Map<BeerResponse>(beerModel)).Returns(beerResponse);

            var result = await _controller.CreateBeer(createBeerRequest);

            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result.Result as CreatedAtActionResult;
            var createdBeer = createdResult?.Value as BeerResponse;

            Assert.That(createdBeer?.Name, Is.EqualTo("New Beer"));
            Assert.That(createdBeer?.Id, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests CreateBeer returns BadRequest when request is null.
        /// </summary>
        [Test]
        public async Task CreateBeer_NullRequest_ReturnsBadRequest()
        {
            var result = await _controller.CreateBeer(null);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        #endregion

        #region UpdateBeer Tests

        /// <summary>
        /// Tests updating an existing beer returns OkObjectResult.
        /// </summary>
        [Test]
        public async Task UpdateBeer_ExistingBeer_ReturnsOkResult()
        {
            int beerId = 1;
            var updateBeerRequest = new CreateBeerRequest { Name = "Updated Beer", PercentageAlcoholByVolume = 6.0 };
            var updateBeerModel = new CreateBeerModel { Name = "Updated Beer", PercentageAlcoholByVolume = 6.0 };

            _mockMapper.Setup(m => m.Map<CreateBeerModel>(updateBeerRequest)).Returns(updateBeerModel);
            _mockBeerProcess.Setup(x => x.UpdateBeer(beerId, updateBeerModel)).ReturnsAsync(true);

            var result = await _controller.UpdateBeer(beerId, updateBeerRequest);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value;
            var messageProp = response?.GetType().GetProperty("message")?.GetValue(response)?.ToString();

            Assert.That(messageProp, Is.EqualTo("Beer updated successfully."));
        }

        /// <summary>
        /// Tests UpdateBeer returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task UpdateBeer_InvalidId_ReturnsBadRequest()
        {
            var updateBeerRequest = new CreateBeerRequest { Name = "Updated Beer" };

            var result = await _controller.UpdateBeer(0, updateBeerRequest);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests UpdateBeer returns BadRequest when request is null.
        /// </summary>
        [Test]
        public async Task UpdateBeer_NullRequest_ReturnsBadRequest()
        {
            var result = await _controller.UpdateBeer(1, null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests updating a non-existent beer returns NotFound.
        /// </summary>
        [Test]
        public async Task UpdateBeer_NonExistentBeer_ReturnsNotFound()
        {
            int beerId = 999;
            var updateBeerRequest = new CreateBeerRequest { Name = "Updated Beer" };
            var updateBeerModel = new CreateBeerModel { Name = "Updated Beer" };

            _mockMapper.Setup(m => m.Map<CreateBeerModel>(updateBeerRequest)).Returns(updateBeerModel);
            _mockBeerProcess.Setup(x => x.UpdateBeer(beerId, updateBeerModel)).ReturnsAsync(false);

            var result = await _controller.UpdateBeer(beerId, updateBeerRequest);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region DeleteBeer Tests

        /// <summary>
        /// Tests deleting an existing beer returns OkObjectResult.
        /// </summary>
        [Test]
        public async Task DeleteBeer_ExistingBeer_ReturnsOkResult()
        {
            int beerId = 1;

            _mockBeerProcess.Setup(x => x.DeleteBeer(beerId)).ReturnsAsync(true);

            var result = await _controller.DeleteBeer(beerId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value;
            var messageProp = response?.GetType().GetProperty("message")?.GetValue(response)?.ToString();

            Assert.That(messageProp, Is.EqualTo("Beer deleted successfully."));
        }

        /// <summary>
        /// Tests DeleteBeer returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task DeleteBeer_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.DeleteBeer(0);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests deleting a non-existent beer returns NotFound.
        /// </summary>
        [Test]
        public async Task DeleteBeer_NonExistentBeer_ReturnsNotFound()
        {
            int beerId = 999;

            _mockBeerProcess.Setup(x => x.DeleteBeer(beerId)).ReturnsAsync(false);

            var result = await _controller.DeleteBeer(beerId);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion
    }
}