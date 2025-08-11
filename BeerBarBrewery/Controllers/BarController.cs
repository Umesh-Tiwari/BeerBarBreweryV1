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
        /// The GetAllBars method will return all the bar records. In case there is no record it will return not found with error message.
        /// </summary>
        /// <returns>if record is present it will return only List of bar details with OK response and return NotFound response(ErrorResponse) when there are no records present</returns>
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
        /// Retrieves a specific bar details by its ID.
        /// </summary>
        /// <param name="id">Bar id for which the record to be displayed</param>
        /// <returns>returns BarResponse object with HTTP Ok response if the record is present  
        /// or return NotFound response(ErrorResponse) when there is no records present or 
        /// will return bad request if id is invalid. Valid id should be greater than 0 </returns>
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
        /// Retrieves all bars with their associated beers.
        /// </summary>
        /// <returns>List of bars with beer details or NotFound if no records exist</returns>
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
        /// Assigns a beer to a bar.
        /// </summary>
        /// <param name="barBeerRequest">Bar and beer IDs to link</param>
        /// <returns>Success message or error response</returns>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [HttpPost("beer")]
        public async Task<IActionResult> AssignBeerToBar(BarBeerRequest barBeerRequest)
        {
            if (barBeerRequest == null)
                return BadRequest(ErrorResponse("BarBeerRequest data cannot be null.", StatusCodes.Status400BadRequest));

            if (!IsValidId(barBeerRequest.BarId) || !IsValidId(barBeerRequest.BeerId))
                return BadRequest(ErrorResponse("Invalid BarId or BeerId.", StatusCodes.Status400BadRequest));

            var result = await _barProcess.LinkBarToBeer(barBeerRequest);
            if (!result)
                return NotFound(ErrorResponse($"Bar with ID {barBeerRequest.BarId} or Beer with ID {barBeerRequest.BeerId} " +
                    $"not found.", StatusCodes.Status404NotFound));

            return Ok(new { message = "Beer assigned to bar successfully." });
        }

        /// <summary>
        /// Gets all beers served at a specific bar.
        /// </summary>
        /// <param name="barId">Bar ID to get beers for</param>
        /// <returns>List of beers served at the bar or error response</returns>
        [ProducesResponseType(typeof(IEnumerable<BeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [HttpGet("{barId}/beer")]
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
        /// Creates a new bar.
        /// </summary>
        /// <param name="createBarRequest">Bar data to create</param>
        /// <returns>Created bar details or Bad Request if data is invalid</returns>
        [HttpPost]
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
        /// Updates an existing bar.
        /// </summary>
        /// <param name="id">Bar ID to update</param>
        /// <param name="updateBarRequest">Updated bar data</param>
        /// <returns>Success message or error response</returns>
        [HttpPut("{id}")]
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
        /// Deletes a bar by ID.
        /// </summary>
        /// <param name="id">Bar ID to delete</param>
        /// <returns>Success message or error response</returns>
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