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
    /// Unit tests for the BreweryController, verifying controller actions with mocked dependencies.
    /// Uses NUnit, Moq, and AutoMapper.
    /// </summary>
    public class BreweryControllerTests
    {
        private readonly Mock<IBreweryProcess> _mockBreweryProcess;
        private readonly BreweryController _controller;
        private readonly Mock<IMapper> _mockMapper;

        /// <summary>
        /// Initializes mocks and the controller for testing.
        /// </summary>
        public BreweryControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockBreweryProcess = new Mock<IBreweryProcess>();
            _controller = new BreweryController(_mockBreweryProcess.Object, _mockMapper.Object);
        }

        #region GetAllBreweries Tests

        /// <summary>
        /// Tests that GetAllBreweries returns OkObjectResult with a list of BreweryResponse when breweries exist.
        /// </summary>
        [Test]
        public async Task GetAllBreweries_ReturnsOkWithBreweryResponses()
        {
            var breweryModels = new List<BreweryModel>
            {
                new BreweryModel { Id = 1, Name = "Brewery One" },
                new BreweryModel { Id = 2, Name = "Brewery Two" }
            };

            var breweryResponses = new List<BreweryResponse>
            {
                new BreweryResponse { Id = 1, Name = "Brewery One" },
                new BreweryResponse { Id = 2, Name = "Brewery Two" }
            };

            _mockBreweryProcess.Setup(x => x.GetAllBreweries()).ReturnsAsync(breweryModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<BreweryResponse>>(breweryModels)).Returns(breweryResponses);

            var result = await _controller.GetAllBreweries();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(breweryResponses));
        }

        /// <summary>
        /// Tests that GetAllBreweries returns NotFound when no breweries exist.
        /// </summary>
        [Test]
        public async Task GetAllBreweries_NoBreweries_ReturnsNotFound()
        {
            var emptyBreweryList = new List<BreweryModel>();

            _mockBreweryProcess.Setup(x => x.GetAllBreweries()).ReturnsAsync(emptyBreweryList);

            var result = await _controller.GetAllBreweries();

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region GetAllBreweriesWithBeer Tests

        /// <summary>
        /// Tests that GetAllBreweriesWithBeer returns OkObjectResult with breweries and their beers.
        /// </summary>
        [Test]
        public async Task GetAllBreweriesWithBeer_ReturnsOkWithBreweriesAndBeers()
        {
            var breweryModels = new List<BreweryModel>
            {
                new BreweryModel { Id = 1, Name = "Brewery One" }
            };

            var breweryWithBeerResponses = new List<BreweryWithBeerResponse>
            {
                new BreweryWithBeerResponse { Id = 1, Name = "Brewery One" }
            };

            _mockBreweryProcess.Setup(x => x.GetAllWithBeerAsync()).ReturnsAsync(breweryModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<BreweryWithBeerResponse>>(breweryModels)).Returns(breweryWithBeerResponses);

            var result = await _controller.GetAllBreweriesWithBeer();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(breweryWithBeerResponses));
        }

        /// <summary>
        /// Tests that GetAllBreweriesWithBeer returns NotFound when no breweries exist.
        /// </summary>
        [Test]
        public async Task GetAllBreweriesWithBeer_NoBreweries_ReturnsNotFound()
        {
            var emptyBreweryList = new List<BreweryModel>();

            _mockBreweryProcess.Setup(x => x.GetAllWithBeerAsync()).ReturnsAsync(emptyBreweryList);

            var result = await _controller.GetAllBreweriesWithBeer();

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region GetBreweryById Tests

        /// <summary>
        /// Tests GetBreweryById returns a mapped BreweryResponse when brewery exists.
        /// </summary>
        [Test]
        public async Task GetBreweryById_ValidId_ReturnsMappedBreweryResponse()
        {
            var breweryModel = new BreweryModel { Id = 1, Name = "Test Brewery" };
            var breweryResponse = new BreweryResponse { Id = 1, Name = "Test Brewery" };

            _mockBreweryProcess.Setup(x => x.GetBreweryById(1)).ReturnsAsync(breweryModel);
            _mockMapper.Setup(m => m.Map<BreweryResponse>(breweryModel)).Returns(breweryResponse);

            var result = await _controller.GetBreweryById(1);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(breweryResponse));
        }

        /// <summary>
        /// Tests GetBreweryById returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task GetBreweryById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.GetBreweryById(0);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBreweryById returns NotFound when brewery doesn't exist.
        /// </summary>
        [Test]
        public async Task GetBreweryById_BreweryNotFound_ReturnsNotFound()
        {
            _mockBreweryProcess.Setup(x => x.GetBreweryById(999)).ReturnsAsync((BreweryModel)null);

            var result = await _controller.GetBreweryById(999);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region GetBreweryWithBeerById Tests

        /// <summary>
        /// Tests GetBreweryWithBeerById returns brewery with beer details.
        /// </summary>
        [Test]
        public async Task GetBreweryWithBeerById_ValidId_ReturnsBreweryWithBeer()
        {
            var breweryModel = new BreweryModel { Id = 1, Name = "Test Brewery" };
            var breweryWithBeerResponse = new BreweryWithBeerResponse { Id = 1, Name = "Test Brewery" };

            _mockBreweryProcess.Setup(x => x.GetBreweryById(1)).ReturnsAsync(breweryModel);
            _mockMapper.Setup(m => m.Map<BreweryWithBeerResponse>(breweryModel)).Returns(breweryWithBeerResponse);

            var result = await _controller.GetBreweryWithBeerById(1);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(breweryWithBeerResponse));
        }

        /// <summary>
        /// Tests GetBreweryWithBeerById returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task GetBreweryWithBeerById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.GetBreweryWithBeerById(0);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBreweryWithBeerById returns NotFound when brewery doesn't exist.
        /// </summary>
        [Test]
        public async Task GetBreweryWithBeerById_BreweryNotFound_ReturnsNotFound()
        {
            _mockBreweryProcess.Setup(x => x.GetBreweryById(999)).ReturnsAsync((BreweryModel)null);

            var result = await _controller.GetBreweryWithBeerById(999);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region CreateBrewery Tests

        /// <summary>
        /// Tests creating a brewery returns CreatedAtActionResult with the new BreweryResponse.
        /// </summary>
        [Test]
        public async Task CreateBrewery_ValidRequest_ReturnsCreatedAtAction()
        {
            var createBreweryRequest = new CreateBreweryRequest { Name = "New Brewery" };
            var createBreweryModel = new CreateBreweryModel { Name = "New Brewery" };
            var breweryModel = new BreweryModel { Id = 1, Name = "New Brewery" };
            var breweryResponse = new BreweryResponse { Id = 1, Name = "New Brewery" };

            _mockMapper.Setup(m => m.Map<CreateBreweryModel>(createBreweryRequest)).Returns(createBreweryModel);
            _mockBreweryProcess.Setup(x => x.CreateBrewery(createBreweryModel)).ReturnsAsync(breweryModel);
            _mockMapper.Setup(m => m.Map<BreweryResponse>(breweryModel)).Returns(breweryResponse);

            var result = await _controller.CreateBrewery(createBreweryRequest);

            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result.Result as CreatedAtActionResult;
            var createdBrewery = createdResult?.Value as BreweryResponse;

            Assert.That(createdBrewery?.Name, Is.EqualTo("New Brewery"));
            Assert.That(createdBrewery?.Id, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests CreateBrewery returns BadRequest when request is null.
        /// </summary>
        [Test]
        public async Task CreateBrewery_NullRequest_ReturnsBadRequest()
        {
            var result = await _controller.CreateBrewery(null);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        #endregion

        #region UpdateBrewery Tests

        /// <summary>
        /// Tests updating an existing brewery returns OkObjectResult.
        /// </summary>
        [Test]
        public async Task UpdateBrewery_ExistingBrewery_ReturnsOkResult()
        {
            int breweryId = 1;
            var updateBreweryRequest = new CreateBreweryRequest { Name = "Updated Brewery" };
            var updateBreweryModel = new CreateBreweryModel { Name = "Updated Brewery" };

            _mockMapper.Setup(m => m.Map<CreateBreweryModel>(updateBreweryRequest)).Returns(updateBreweryModel);
            _mockBreweryProcess.Setup(x => x.UpdateBrewery(breweryId, updateBreweryModel)).ReturnsAsync(true);

            var result = await _controller.UpdateBrewery(breweryId, updateBreweryRequest);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value;
            var messageProp = response?.GetType().GetProperty("message")?.GetValue(response)?.ToString();

            Assert.That(messageProp, Is.EqualTo("Brewery updated successfully."));
        }

        /// <summary>
        /// Tests UpdateBrewery returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task UpdateBrewery_InvalidId_ReturnsBadRequest()
        {
            var updateBreweryRequest = new CreateBreweryRequest { Name = "Updated Brewery" };

            var result = await _controller.UpdateBrewery(0, updateBreweryRequest);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests UpdateBrewery returns BadRequest when request is null.
        /// </summary>
        [Test]
        public async Task UpdateBrewery_NullRequest_ReturnsBadRequest()
        {
            var result = await _controller.UpdateBrewery(1, null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests updating a non-existent brewery returns NotFound.
        /// </summary>
        [Test]
        public async Task UpdateBrewery_NonExistentBrewery_ReturnsNotFound()
        {
            int breweryId = 999;
            var updateBreweryRequest = new CreateBreweryRequest { Name = "Updated Brewery" };
            var updateBreweryModel = new CreateBreweryModel { Name = "Updated Brewery" };

            _mockMapper.Setup(m => m.Map<CreateBreweryModel>(updateBreweryRequest)).Returns(updateBreweryModel);
            _mockBreweryProcess.Setup(x => x.UpdateBrewery(breweryId, updateBreweryModel)).ReturnsAsync(false);

            var result = await _controller.UpdateBrewery(breweryId, updateBreweryRequest);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region DeleteBrewery Tests

        /// <summary>
        /// Tests deleting an existing brewery returns OkObjectResult.
        /// </summary>
        [Test]
        public async Task DeleteBrewery_ExistingBrewery_ReturnsOkResult()
        {
            int breweryId = 1;

            _mockBreweryProcess.Setup(x => x.DeleteBrewery(breweryId)).ReturnsAsync(true);

            var result = await _controller.DeleteBrewery(breweryId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value;
            var messageProp = response?.GetType().GetProperty("message")?.GetValue(response)?.ToString();

            Assert.That(messageProp, Is.EqualTo("Brewery deleted successfully."));
        }

        /// <summary>
        /// Tests DeleteBrewery returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task DeleteBrewery_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.DeleteBrewery(0);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests deleting a non-existent brewery returns NotFound.
        /// </summary>
        [Test]
        public async Task DeleteBrewery_NonExistentBrewery_ReturnsNotFound()
        {
            int breweryId = 999;

            _mockBreweryProcess.Setup(x => x.DeleteBrewery(breweryId)).ReturnsAsync(false);

            var result = await _controller.DeleteBrewery(breweryId);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion

        #region AssignBreweryToBeer Tests

        /// <summary>
        /// Tests assigning brewery to beer returns OkObjectResult.
        /// </summary>
        [Test]
        public async Task AssignBreweryToBeer_ValidRequest_ReturnsOkResult()
        {
            var breweryWithBeerRequest = new BreweryWithBeerRequest { BreweryId = 1, BeerId = 1 };
            var breweryBeerModel = new BreweryBeerModel { BreweryId = 1, BeerId = 1 };

            _mockMapper.Setup(m => m.Map<BreweryBeerModel>(breweryWithBeerRequest)).Returns(breweryBeerModel);
            _mockBreweryProcess.Setup(x => x.AssignBreweryToBeer(breweryBeerModel)).ReturnsAsync(true);

            var result = await _controller.AssignBreweryToBeer(breweryWithBeerRequest);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value;
            var messageProp = response?.GetType().GetProperty("message")?.GetValue(response)?.ToString();

            Assert.That(messageProp, Is.EqualTo("Brewery assigned to beer successfully."));
        }

        /// <summary>
        /// Tests AssignBreweryToBeer returns BadRequest when request is null.
        /// </summary>
        [Test]
        public async Task AssignBreweryToBeer_NullRequest_ReturnsBadRequest()
        {
            var result = await _controller.AssignBreweryToBeer(null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests AssignBreweryToBeer returns BadRequest for invalid IDs.
        /// </summary>
        [Test]
        public async Task AssignBreweryToBeer_InvalidIds_ReturnsBadRequest()
        {
            var breweryWithBeerRequest = new BreweryWithBeerRequest { BreweryId = 0, BeerId = 1 };

            var result = await _controller.AssignBreweryToBeer(breweryWithBeerRequest);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests AssignBreweryToBeer returns NotFound when brewery or beer not found.
        /// </summary>
        [Test]
        public async Task AssignBreweryToBeer_BreweryOrBeerNotFound_ReturnsNotFound()
        {
            var breweryWithBeerRequest = new BreweryWithBeerRequest { BreweryId = 999, BeerId = 999 };
            var breweryBeerModel = new BreweryBeerModel { BreweryId = 999, BeerId = 999 };

            _mockMapper.Setup(m => m.Map<BreweryBeerModel>(breweryWithBeerRequest)).Returns(breweryBeerModel);
            _mockBreweryProcess.Setup(x => x.AssignBreweryToBeer(breweryBeerModel)).ReturnsAsync(false);

            var result = await _controller.AssignBreweryToBeer(breweryWithBeerRequest);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion
    }
}