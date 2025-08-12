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
                new BeerModel { Id = 1, Name = "Beer One", PercentageAlcoholByVolume = 5.0M },
                new BeerModel { Id = 2, Name = "Beer Two", PercentageAlcoholByVolume = 4.5M }
            };

            var beerResponses = new List<BeerResponse>
            {
                new BeerResponse { Id = 1, Name = "Beer One", PercentageAlcoholByVolume = 5.0M },
                new BeerResponse { Id = 2, Name = "Beer Two", PercentageAlcoholByVolume = 4.5M }
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
            var beerModel = new BeerModel { Id = 1, Name = "Test Beer", PercentageAlcoholByVolume = 5.0M };
            var beerResponse = new BeerResponse { Id = 1, Name = "Test Beer", PercentageAlcoholByVolume = 5.0M };

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
                new BeerModel { Id = 1, Name = "Light Beer", PercentageAlcoholByVolume = 4.0M }
            };

            var beerResponses = new List<BeerResponse>
            {
                new BeerResponse { Id = 1, Name = "Light Beer", PercentageAlcoholByVolume = 4.0M }
            };

            _mockBeerProcess.Setup(x => x.GetBeersByAlcoholVolumeRange(3.0M, 5.0M)).ReturnsAsync(beerModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeerResponse>>(beerModels)).Returns(beerResponses);

            var result = await _controller.GetBeersByAlcoholVolumeRange(3.0M, 5.0M);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(beerResponses));
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns BadRequest when both parameters are null.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_BothParametersNull_ReturnsBadRequest()
        {
            var result = await _controller.GetBeersByAlcoholVolumeRange(null, null);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns BadRequest for negative minimum value.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_NegativeMinimum_ReturnsBadRequest()
        {
            var result = await _controller.GetBeersByAlcoholVolumeRange(-1.0M, 5.0M);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns BadRequest for negative maximum value.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_NegativeMaximum_ReturnsBadRequest()
        {
            var result = await _controller.GetBeersByAlcoholVolumeRange(3.0M, -2.0M);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns BadRequest when min >= max.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_MinimumGreaterThanMaximum_ReturnsBadRequest()
        {
            var result = await _controller.GetBeersByAlcoholVolumeRange(5.0M, 3.0M);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns BadRequest when min equals max.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_MinimumEqualsMaximum_ReturnsBadRequest()
        {
            var result = await _controller.GetBeersByAlcoholVolumeRange(5.0M, 5.0M);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns NotFound when no beers match criteria.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_NoBeersMatchCriteria_ReturnsNotFound()
        {
            var emptyBeerList = new List<BeerModel>();

            _mockBeerProcess.Setup(x => x.GetBeersByAlcoholVolumeRange(10.0M, 15.0M)).ReturnsAsync(emptyBeerList);

            var result = await _controller.GetBeersByAlcoholVolumeRange(10.0M, 15.0M);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns beers when only minimum parameter is provided.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_OnlyMinimumProvided_ReturnsBeers()
        {
            var beerModels = new List<BeerModel>
            {
                new BeerModel { Id = 1, Name = "Strong Beer", PercentageAlcoholByVolume = 8.0M }
            };

            var beerResponses = new List<BeerResponse>
            {
                new BeerResponse { Id = 1, Name = "Strong Beer", PercentageAlcoholByVolume = 8.0M }
            };

            _mockBeerProcess.Setup(x => x.GetBeersByAlcoholVolumeRange(5.0M, null)).ReturnsAsync(beerModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeerResponse>>(beerModels)).Returns(beerResponses);

            var result = await _controller.GetBeersByAlcoholVolumeRange(5.0M, null);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(beerResponses));
        }

        /// <summary>
        /// Tests GetBeersByAlcoholVolumeRange returns beers when only maximum parameter is provided.
        /// </summary>
        [Test]
        public async Task GetBeersByAlcoholVolumeRange_OnlyMaximumProvided_ReturnsBeers()
        {
            var beerModels = new List<BeerModel>
            {
                new BeerModel { Id = 1, Name = "Light Beer", PercentageAlcoholByVolume = 3.0M }
            };

            var beerResponses = new List<BeerResponse>
            {
                new BeerResponse { Id = 1, Name = "Light Beer", PercentageAlcoholByVolume = 3.0M }
            };

            _mockBeerProcess.Setup(x => x.GetBeersByAlcoholVolumeRange(null, 4.0M)).ReturnsAsync(beerModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeerResponse>>(beerModels)).Returns(beerResponses);

            var result = await _controller.GetBeersByAlcoholVolumeRange(null, 4.0M);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(beerResponses));
        }

        #endregion

        #region CreateBeer Tests

        /// <summary>
        /// Tests creating a beer returns CreatedAtActionResult with the new BeerResponse.
        /// </summary>
        [Test]
        public async Task CreateBeer_ValidRequest_ReturnsCreatedAtAction()
        {
            var createBeerRequest = new CreateBeerRequest { Name = "New Beer", PercentageAlcoholByVolume = 5.0M };
            var createBeerModel = new CreateBeerModel { Name = "New Beer", PercentageAlcoholByVolume = 5.0M };
            var beerModel = new BeerModel { Id = 1, Name = "New Beer", PercentageAlcoholByVolume = 5.0M };
            var beerResponse = new BeerResponse { Id = 1, Name = "New Beer", PercentageAlcoholByVolume = 5.0M };

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
            var updateBeerRequest = new CreateBeerRequest { Name = "Updated Beer", PercentageAlcoholByVolume = 6.0M };
            var updateBeerModel = new CreateBeerModel { Name = "Updated Beer", PercentageAlcoholByVolume = 6.0M };

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