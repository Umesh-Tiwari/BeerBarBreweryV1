using AutoMapper;
using BeerBarBrewery.Controllers;
using Business.BeerBarBrewery.Process.Interface;
using Contract.BeerBarBrewery;
using Microsoft.AspNetCore.Mvc;
using Model.BeerBarBrewery;
using Moq;
using Models = Model.BeerBarBrewery;

namespace BeerBarBrewery.Tests.Controller
{
    /// <summary>
    /// Unit tests for the BarController, verifying controller actions with mocked dependencies.
    /// Uses NUnit, Moq, and AutoMapper.
    /// </summary>
    public class BarControllerTests
    {
        private readonly Mock<IBarProcess> _mockBarProcess;
        private readonly BarController _controller;
        private readonly Mock<IMapper> _mockMapper;

        /// <summary>
        /// Initializes mocks and the controller for testing.
        /// </summary>
        public BarControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockBarProcess = new Mock<IBarProcess>();
            _controller = new BarController(_mockBarProcess.Object, _mockMapper.Object);
        }

        /// <summary>
        /// Tests GetBarById returns a mapped DTO when bar exists.
        /// </summary>
        [Test]
        public async Task GetBarById_ValidId_ReturnsMappedDto()
        {
            var bar = new Models.BarModel { Id = 1, Name = "Test Bar" };
            var barDto = new BarResponse { Id = 1, Name = "Test Bar" };

            _mockBarProcess.Setup(r => r.GetBarById(1)).ReturnsAsync(bar);
            _mockMapper.Setup(m => m.Map<BarResponse>(bar)).Returns(barDto);

            var result = await _controller.GetBarById(1);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        /// <summary>
        /// Tests GetBarById returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task GetBarById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.GetBarById(0);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBarById returns NotFound when bar doesn't exist.
        /// </summary>
        [Test]
        public async Task GetBarById_BarNotFound_ReturnsNotFound()
        {
            _mockBarProcess.Setup(r => r.GetBarById(999)).ReturnsAsync((BarModel)null);

            var result = await _controller.GetBarById(999);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests that GetAllBars returns OkObjectResult with a list of BarDto.
        /// </summary>
        [Test]
        public async Task GetAllBars_ReturnsOkWithBarDtos()
        {
            var barsModel = new List<BarModel>
        {
            new BarModel { Id = 1, Name = "Bar One" },
            new BarModel { Id = 2, Name = "Bar Two" }
        };

            var barDtos = new List<BarResponse>
        {
            new BarResponse { Id = 1, Name = "Bar One" },
            new BarResponse { Id = 2, Name = "Bar Two" }
        };

            _mockBarProcess.Setup(u => u.GetAllBars()).ReturnsAsync(barsModel);
            _mockMapper.Setup(m => m.Map<IEnumerable<BarResponse>>(barsModel)).Returns(barDtos);

            var result = await _controller.GetAllBars();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(barDtos));
        }

        /// <summary>
        /// Tests that GetAllBars returns NotFound when no bars exist.
        /// </summary>
        [Test]
        public async Task GetAllBars_NoBars_ReturnsNotFound()
        {
            var emptyBarList = new List<BarModel>();

            _mockBarProcess.Setup(u => u.GetAllBars()).ReturnsAsync(emptyBarList);

            var result = await _controller.GetAllBars();

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests that GetAllBarsWithBeers returns OkObjectResult with bars and their beers.
        /// </summary>
        [Test]
        public async Task GetAllBarsWithBeers_ReturnsOkWithBarsAndBeers()
        {
            var barModels = new List<BarModel>
            {
                new BarModel { Id = 1, Name = "Bar One" }
            };

            var barWithBeerResponses = new List<BarWithBeerResponse>
            {
                new BarWithBeerResponse { Id = 1, Name = "Bar One" }
            };

            _mockBarProcess.Setup(u => u.GetAllBarsWithBeers()).ReturnsAsync(barModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<BarWithBeerResponse>>(barModels)).Returns(barWithBeerResponses);

            var result = await _controller.GetAllBarsWithBeers();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(barWithBeerResponses));
        }

        /// <summary>
        /// Tests that GetAllBarsWithBeers returns NotFound when no bars exist.
        /// </summary>
        [Test]
        public async Task GetAllBarsWithBeers_NoBars_ReturnsNotFound()
        {
            var emptyBarList = new List<BarModel>();

            _mockBarProcess.Setup(u => u.GetAllBarsWithBeers()).ReturnsAsync(emptyBarList);

            var result = await _controller.GetAllBarsWithBeers();

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests creating a bar returns CreatedAtActionResult with the new BarDto.
        /// </summary>
        [Test]
        public async Task CreateBar_ValidDto_ReturnsCreatedAtAction()
        {
            var barDto = new CreateBarRequest { Name = "New Bar" };
            var createBarModel = new CreateBarModel { Name = "New Bar" };
            var barModel = new BarModel { Id = 1, Name = "New Bar" };

            _mockMapper.Setup(m => m.Map<CreateBarModel>(barDto)).Returns(createBarModel);
            _mockBarProcess.Setup(u => u.CreateBar(createBarModel)).ReturnsAsync(barModel);
            _mockMapper.Setup(m => m.Map<BarResponse>(barModel)).Returns(new BarResponse { Id = 1, Name = "New Bar" });

            var result = await _controller.CreateBar(barDto);

            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result.Result as CreatedAtActionResult;
            var createdDto = createdResult?.Value as BarResponse;

            Assert.That(createdDto?.Name, Is.EqualTo("New Bar"));
            Assert.That(createdDto?.Id, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests CreateBar returns BadRequest when request is null.
        /// </summary>
        [Test]
        public async Task CreateBar_NullRequest_ReturnsBadRequest()
        {
            var result = await _controller.CreateBar(null);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests CreateBar returns BadRequest when model state is invalid.
        /// </summary>
        [Test]
        public async Task CreateBar_InvalidModelState_ReturnsBadRequest()
        {
            var createBarRequest = new CreateBarRequest { Name = "" };
            _controller.ModelState.AddModelError("Name", "Name is required");

            var result = await _controller.CreateBar(createBarRequest);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests updating an existing bar returns ok object results.
        /// </summary>
        [Test]
        public async Task UpdateBar_ExistingBar_UpdatesAndReturnsOkResult()
        {
            int barId = 1;
            var barDto = new CreateBarRequest { Name = "New Bar" };
            var updateBarModel = new CreateBarModel { Name = "New Bar" };

            _mockMapper.Setup(m => m.Map<CreateBarModel>(barDto)).Returns(updateBarModel);
            _mockBarProcess.Setup(u => u.UpdateBar(barId, updateBarModel)).ReturnsAsync(true);

            var result = await _controller.UpdateBar(barId, barDto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            // Extract anonymous object and compare message
            var response = okResult.Value;
            var messageProp = response?.GetType().GetProperty("message")?.GetValue(response)?.ToString();

            Assert.That(messageProp, Is.EqualTo("Bar updated successfully."));
        }

        /// <summary>
        /// Tests updating a non-existent bar returns NotFound.
        /// </summary>
        [Test]
        public async Task UpdateBar_NonExistentBar_ReturnsNotFound()
        {
            int barId = 999;
            var barDto = new CreateBarRequest { Name = "New Bar" };
            var updateBarModel = new CreateBarModel { Name = "New Bar" };

            _mockMapper.Setup(m => m.Map<CreateBarModel>(barDto)).Returns(updateBarModel);
            _mockBarProcess.Setup(u => u.UpdateBar(barId, updateBarModel)).ReturnsAsync(false);

            var result = await _controller.UpdateBar(barId, barDto);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests UpdateBar returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task UpdateBar_InvalidId_ReturnsBadRequest()
        {
            var updateBarRequest = new CreateBarRequest { Name = "Updated Bar" };

            var result = await _controller.UpdateBar(0, updateBarRequest);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests UpdateBar returns BadRequest when request is null.
        /// </summary>
        [Test]
        public async Task UpdateBar_NullRequest_ReturnsBadRequest()
        {
            var result = await _controller.UpdateBar(1, null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests deleting an existing bar returns NoContent.
        /// </summary>
        [Test]
        public async Task DeleteBar_ExistingBar_DeletesAndReturnsNoContent()
        {
            int barId = 1;

            _mockBarProcess.Setup(u => u.DeleteBar(barId)).ReturnsAsync(true);

            var result = await _controller.DeleteBar(barId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        /// <summary>
        /// Tests deleting a non-existent bar returns NotFound.
        /// </summary>
        [Test]
        public async Task DeleteBar_NonExistentBar_ReturnsNotFound()
        {
            int barId = 999;

            _mockBarProcess.Setup(u => u.DeleteBar(barId)).ReturnsAsync(false);

            var result = await _controller.DeleteBar(barId);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests DeleteBar returns BadRequest for invalid ID.
        /// </summary>
        [Test]
        public async Task DeleteBar_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.DeleteBar(0);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests assigning a beer to a bar returns Ok if successful.
        /// </summary>
        [Test]
        public async Task AssignBeerToBar_ValidIds_AssignsBeer_ReturnsOk()
        {
            int barId = 1, beerId = 10;
            var barBeerLinkDto = new BarBeerRequest { BarId = barId, BeerId = beerId };

            _mockBarProcess.Setup(u => u.LinkBarToBeer(barBeerLinkDto)).ReturnsAsync(AssignmentResult.Success);

            var result = await _controller.AssignBeerToBar(barBeerLinkDto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            // Extract anonymous object and compare message
            var response = okResult.Value;
            var messageProp = response?.GetType().GetProperty("message")?.GetValue(response)?.ToString();

            Assert.That(messageProp, Is.EqualTo("Beer assigned to bar successfully."));
        }

        /// <summary>
        /// Tests assigning beer to a non-existent bar returns NotFound.
        /// </summary>
        [Test]
        public async Task AssignBeerToBar_BarNotFound_ReturnsNotFound()
        {
            int barId = 999, beerId = 10;
            var barBeerLinkDto = new BarBeerRequest { BarId = barId, BeerId = beerId };

            _mockBarProcess.Setup(u => u.LinkBarToBeer(barBeerLinkDto)).ReturnsAsync(AssignmentResult.NotFound);

            var result = await _controller.AssignBeerToBar(barBeerLinkDto);
            // Cast result to ObjectResult
            var objectResult = result as ObjectResult;

            // Assert it’s not null and status code is 404
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(404));

        }

        /// <summary>
        /// Tests assigning a non-existent beer to a bar returns NotFound.
        /// </summary>
        [Test]
        public async Task AssignBeerToBar_BeerNotFound_ReturnsNotFound()
        {
            int barId = 1, beerId = 999;
            var barBeerLinkDto = new BarBeerRequest { BarId = barId, BeerId = beerId };

            _mockBarProcess.Setup(u => u.LinkBarToBeer(barBeerLinkDto)).ReturnsAsync(AssignmentResult.NotFound);

            var result = await _controller.AssignBeerToBar(barBeerLinkDto);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests assigning a beer that is already assigned to a bar returns Ok with appropriate message.
        /// </summary>
        [Test]
        public async Task AssignBeerToBar_BeerAlreadyAssigned_ReturnsOk()
        {
            int barId = 1, beerId = 10;
            var barBeerLinkDto = new BarBeerRequest { BarId = barId, BeerId = beerId };

            _mockBarProcess.Setup(u => u.LinkBarToBeer(barBeerLinkDto)).ReturnsAsync(AssignmentResult.AlreadyExists);

            var result = await _controller.AssignBeerToBar(barBeerLinkDto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value;
            var messageProp = response?.GetType().GetProperty("message")?.GetValue(response)?.ToString();

            Assert.That(messageProp, Is.EqualTo("Beer already assigned to bar."));
        }

        /// <summary>
        /// Tests AssignBeerToBar returns BadRequest when request is null.
        /// </summary>
        [Test]
        public async Task AssignBeerToBar_NullRequest_ReturnsBadRequest()
        {
            var result = await _controller.AssignBeerToBar(null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests AssignBeerToBar returns BadRequest for invalid bar ID.
        /// </summary>
        [Test]
        public async Task AssignBeerToBar_InvalidBarId_ReturnsBadRequest()
        {
            var barBeerRequest = new BarBeerRequest { BarId = 0, BeerId = 1 };

            var result = await _controller.AssignBeerToBar(barBeerRequest);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests AssignBeerToBar returns BadRequest for invalid beer ID.
        /// </summary>
        [Test]
        public async Task AssignBeerToBar_InvalidBeerId_ReturnsBadRequest()
        {
            var barBeerRequest = new BarBeerRequest { BarId = 1, BeerId = 0 };

            var result = await _controller.AssignBeerToBar(barBeerRequest);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests getting beers served at a specific bar returns a list of BeerDto.
        /// </summary>
        [Test]
        public async Task GetBeersServedAtBarAsync_ValidBarId_ReturnsBeers()
        {
            int barId = 1;
            var beerDto = new List<BeerResponse> { new BeerResponse { Id = 2, Name = "New Beer", PercentageAlcoholByVolume = 12.0M } };
            var beerModel = new List<BeerModel> { new BeerModel { Id = 2, Name = "New Beer", PercentageAlcoholByVolume = 12.0M } };

            _mockBarProcess.Setup(u => u.GetBeersServedAtBarAsync(barId)).ReturnsAsync(beerModel);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeerResponse>>(beerModel)).Returns(beerDto);

            var result = await _controller.GetBeersServedAtBar(barId);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(beerDto));
        }

        /// <summary>
        /// Tests GetBeersServedAtBar returns BadRequest for invalid bar ID.
        /// </summary>
        [Test]
        public async Task GetBeersServedAtBar_InvalidBarId_ReturnsBadRequest()
        {
            var result = await _controller.GetBeersServedAtBar(0);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests GetBeersServedAtBar returns NotFound when no beers found for bar.
        /// </summary>
        [Test]
        public async Task GetBeersServedAtBar_NoBeersFound_ReturnsNotFound()
        {
            int barId = 1;
            var emptyBeerList = new List<BeerModel>();

            _mockBarProcess.Setup(u => u.GetBeersServedAtBarAsync(barId)).ReturnsAsync(emptyBeerList);

            var result = await _controller.GetBeersServedAtBar(barId);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }
}
