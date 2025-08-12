using AutoMapper;
using Business.BeerBarBrewery.Process.Interface;
using Contract.BeerBarBrewery;
using Microsoft.AspNetCore.Mvc;
using Model.BeerBarBrewery;
namespace BeerBarBrewery.Controllers
{
    /// <summary>
    /// The BarController handles requests related to bars in the application 
    /// and returns response back to the user. its methods are also used to link bar with beer. It can also display associated beer details
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BarController : BaseController
    {
        /// <summary>
        /// IMapper is an interface from the AutoMapper library used to automatically map object to object. 
        /// Here it is used to map bar contract with model and model with bar contract
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// IBarProcess holds a reference to an object implementing the IBarProcess interface. IBarProcess represents a custom business logic service that handles operations related to Bar entities
        /// </summary>
        private readonly IBarProcess _barProcess;

        /// <summary>
        /// Bar controller instance injecting IBarProcess and IMapper dependency
        /// </summary>
        /// <param name="barProcess"> service object to carry out business process</param>
        /// <param name="mapper"> Mapper used to map object to object. here it is used to map contract object to model object</param>
        public BarController(IBarProcess barProcess, IMapper mapper)
        {
            _barProcess = barProcess;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all bar records. Returns a NotFound response with an error message if no records are found.
        /// </summary>
        /// <returns>
        /// An OK response containing a list of bar details if records are present, 
        /// or a NotFound response with an error message if no records exist.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BarResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BarResponse>>> GetAllBars()
        {
            var barModels = await _barProcess.GetAllBars();
            if (!barModels.Any())
                return NotFound(ErrorResponse("Bar data not found.", StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<IEnumerable<BarResponse>>(barModels));
        }

        /// <summary>
        /// Retrieves the details of a specific bar by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the bar to retrieve. Must be greater than 0.</param>
        /// <returns>
        /// An BarResponse object with an HTTP OK response if the record exists,  
        /// a NotFound response with an ErrorResponse if no record is found,  
        /// or a BadRequest response if the ID is invalid.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BarResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BarResponse>> GetBarById(int id)
        {
            if (!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid bar ID.", StatusCodes.Status400BadRequest));

            var bar = await _barProcess.GetBarById(id);
            if (bar == null)
                return NotFound(ErrorResponse($"Bar with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<BarResponse>(bar));
        }

        /// <summary>
        /// Retrieves all bars along with their associated beer details.
        /// </summary>
        /// <returns>A list of bars with their beer details, or a NotFound response if no records are found.</returns>
        [HttpGet("beer")]
        [ProducesResponseType(typeof(IEnumerable<BarWithBeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BarWithBeerResponse>>> GetAllBarsWithBeers()
        {
            var barModel = await _barProcess.GetAllBarsWithBeers();
            if (!barModel.Any())
                return NotFound(ErrorResponse("No bars with associated beers found.", StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<IEnumerable<BarWithBeerResponse>>(barModel));
        }

        /// <summary>
        /// Assigns a beer to a specific bar.
        /// </summary>
        /// <param name="barBeerRequest">The request object containing the bar and beer IDs to be linked.</param>
        /// <returns>A success message if the assignment is successful, or an error response if it fails.</returns>
        [HttpPost("beer")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignBeerToBar(BarBeerRequest barBeerRequest)
        {
            if (barBeerRequest == null)
                return BadRequest(ErrorResponse("BarBeerRequest data cannot be null.", StatusCodes.Status400BadRequest));

            if (!IsValidId(barBeerRequest.BarId) || !IsValidId(barBeerRequest.BeerId))
                return BadRequest(ErrorResponse("Invalid BarId or BeerId.", StatusCodes.Status400BadRequest));

            var result = await _barProcess.LinkBarToBeer(barBeerRequest);
            
            return result switch
            {
                AssignmentResult.Success => Ok(new { message = "Beer assigned to bar successfully." }),
                AssignmentResult.AlreadyExists => Ok(new { message = "Beer already assigned to bar." }),
                AssignmentResult.NotFound => NotFound(ErrorResponse($"Bar with ID {barBeerRequest.BarId} or Beer with ID {barBeerRequest.BeerId} not found.", StatusCodes.Status404NotFound)),
                _ => BadRequest(ErrorResponse("Unknown error occurred.", StatusCodes.Status400BadRequest))
            };
        }

        /// <summary>
        /// Retrieves all beers served at a specific bar.
        /// </summary>
        /// <param name="barId">The unique identifier of the bar whose beer list is to be retrieved.</param>
        /// <returns>A list of beers served at the specified bar, or an error response if the request fails.</returns>
        [HttpGet("{barId}/beer")]
        [ProducesResponseType(typeof(IEnumerable<BeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BeerResponse>>> GetBeersServedAtBar(int barId)
        {
            if (!IsValidId(barId))
                return BadRequest(ErrorResponse("Invalid bar ID.", StatusCodes.Status400BadRequest));

            var beerModel = await _barProcess.GetBeersServedAtBarAsync(barId);
            if (beerModel == null || !beerModel.Any())
                return NotFound(ErrorResponse($"No beers found for bar ID {barId}.", StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<IEnumerable<BeerResponse>>(beerModel));
        }

        /// <summary>
        /// Creates a new bar with the provided data.
        /// </summary>
        /// <param name="createBarRequest">The request object containing the bar's data.</param>
        /// <returns>The details of the created bar, or a Bad Request response if the input data is invalid.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(BarResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BarResponse>> CreateBar(CreateBarRequest createBarRequest)
        {
            if (createBarRequest == null)
                return BadRequest(ErrorResponse("Bar data parameter cannot be null.", StatusCodes.Status400BadRequest));

            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse("Bar data passed is missing or invalid. Please check the data.", StatusCodes.Status400BadRequest));

            var barModel = await _barProcess.CreateBar(_mapper.Map<CreateBarModel>(createBarRequest));
            var barResponse = _mapper.Map<BarResponse>(barModel);
            return CreatedAtAction(nameof(GetBarById), new { id = barModel.Id }, barResponse);
        }

        /// <summary>
        /// Updates an existing bar with the provided data.
        /// </summary>
        /// <param name="id">The unique identifier of the bar to update.</param>
        /// <param name="updateBarRequest">The request object containing the updated bar data.</param>
        /// <returns>A success message if the update is successful, or an error response if it fails.</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBar(int id, CreateBarRequest updateBarRequest)
        {
            if (!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid bar ID.", StatusCodes.Status400BadRequest));

            if (updateBarRequest == null)
                return BadRequest(ErrorResponse("Bar data parameter cannot be null.", StatusCodes.Status400BadRequest));

            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse("Bar data passed is missing or invalid. Please check the data.", StatusCodes.Status400BadRequest));

            var result = await _barProcess.UpdateBar(id, _mapper.Map<CreateBarModel>(updateBarRequest));
            if (!result)
                return NotFound(ErrorResponse($"Bar with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(new { message = "Bar updated successfully." });
        }

        /// <summary>
        /// Deletes an existing bar by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the bar to be deleted.</param>
        /// <returns>A success message if the deletion is successful, or an error response if it fails.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBar(int id)
        {
            if (!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid bar ID.", StatusCodes.Status400BadRequest));

            var result = await _barProcess.DeleteBar(id);
            if (!result)
                return NotFound(ErrorResponse($"Bar with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(new { message = "Bar record deleted successfully." });
        }
    }
}