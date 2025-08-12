using AutoMapper;
using Business.BeerBarBrewery.Process.Interface;
using Contract.BeerBarBrewery;
using Microsoft.AspNetCore.Mvc;
using Model.BeerBarBrewery;

namespace BeerBarBrewery.Controllers
{
    /// <summary>
    /// The BreweryController is a web API controller class that handles all HTTP requests related to brewery operations in the application.
    /// It is also used to link beer with type of brewery
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BreweryController : BaseController
    {
        /// <summary>
        ///  IBreweryProcess instance holds a reference to a service responsible for handling brewery-related business logic
        /// </summary>
        private readonly IBreweryProcess _breweryProcess;
        /// <summary>
        /// IMapper is an interface from the AutoMapper library used to automatically map object to object. 
        /// Here it is used to map brewery contract with model and model with brewery contract
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// BreweryController Constructor injecting business logic layer and AutoMapper
        /// </summary>
        /// <param name="breweryProcess">Service object to carry out business process </param>
        /// <param name="mapper">Mapper used to map object to object. Here it is used to map contract object to model object</param>
        public BreweryController(IBreweryProcess breweryProcess, IMapper mapper)
        {
            _breweryProcess = breweryProcess;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all brewery records.
        /// Returns a NotFound response with an error message if no records are found.
        /// </summary>
        /// <returns>
        /// - An HTTP OK response containing a list of brewery details if records are present.  
        /// - A NotFound response if no brewery records exist.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BreweryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BreweryResponse>>> GetAllBreweries()
        {
            var breweryModels = await _breweryProcess.GetAllBreweries();
            if (!breweryModels.Any())
                return NotFound(ErrorResponse("Breweries data not found.", StatusCodes.Status404NotFound));
            return Ok(_mapper.Map<IEnumerable<BreweryResponse>>(breweryModels));
        }

        /// <summary>Retrieves all brewery records along with their associated beer records.</summary>
        /// <returns>
        /// - An HTTP OK response containing a list of breweries with their beer details if records are found.  
        /// - A NotFound response if no brewery or beer records exist.
        /// </returns>
        [HttpGet("beer")]
        [ProducesResponseType(typeof(IEnumerable<BreweryWithBeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BreweryWithBeerResponse>>> GetAllBreweriesWithBeer()
        {
            var breweries = await _breweryProcess.GetAllWithBeerAsync();
            if (!breweries.Any())
                return NotFound(ErrorResponse("Breweries data not found.", StatusCodes.Status404NotFound));
            return Ok(_mapper.Map<IEnumerable<BreweryWithBeerResponse>>(breweries));
        }

        /// <summary>
        /// Retrieves a specific brewery by its ID.
        /// </summary>
        /// <param name="id">The ID of the brewery against which the brewery record will be displayed.</param>
        /// <returns> Returns breweryResponse object with HTTP Ok response if the record exists  
        /// or return NotFound response(ErrorResponse) when there are no records present 
        /// or will return bad request if id is invalid. Valid id should be greater than 0 </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BreweryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BreweryResponse>> GetBreweryById(int id)
        {
            if (!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid brewery ID.", StatusCodes.Status400BadRequest));

            var brewery = await _breweryProcess.GetBreweryById(id);
            if (brewery == null)
                return NotFound(ErrorResponse($"Brewery with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<BreweryResponse>(brewery));
        }

        /// <summary>Retrieves a specific brewery along with its associated beer records by brewery ID.</summary>
        /// <param name="breweryId">The ID of the brewery to retrieve. Must be greater than 0.</param>
        /// <returns>An HTTP OK response containing the brewery and its associated beer details if the record exists.  
        /// - A BadRequest response if the brewery ID is invalid (i.e., less than or equal to 0).  
        /// - A NotFound response if no brewery record is found for the specified ID.
        /// </returns>
        [HttpGet("{breweryId}/beer")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BreweryWithBeerResponse>> GetBreweryWithBeerById(int breweryId)
        {
            if (!IsValidId(breweryId))
                return BadRequest(ErrorResponse("Invalid brewery ID.", StatusCodes.Status400BadRequest));

            var brewery = await _breweryProcess.GetBreweryById(breweryId);
            if (brewery == null)
                return NotFound(ErrorResponse($"Brewery with ID {breweryId} not found.", StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<BreweryWithBeerResponse>(brewery));
        }

        /// <summary>Adds a new brewery record using the provided data.</summary>
        /// <param name="createBreweryRequest">The request object containing brewery data to be saved.</param>
        /// <returns>An HTTP Created response if the brewery record is successfully created. A BadRequest response if the input data is null or invalid.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BreweryResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BreweryResponse>> CreateBrewery(CreateBreweryRequest createBreweryRequest)
        {
            if (createBreweryRequest == null)
                return BadRequest(ErrorResponse("Brewery data passed as parameter is null.", StatusCodes.Status400BadRequest));

            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse("Brewery data passed is missing or invalid. Please check the data.", StatusCodes.Status400BadRequest));

            var breweryModel = await _breweryProcess.CreateBrewery(_mapper.Map<CreateBreweryModel>(createBreweryRequest));
            var breweryResponse = _mapper.Map<BreweryResponse>(breweryModel);

            return CreatedAtAction(nameof(GetBreweryById), new { id = breweryModel.Id }, breweryResponse);
        }

        /// <summary>Updates an existing brewery record by its ID using the provided data.</summary>
        /// <param name="id">The ID of the brewery to update. Must be greater than 0.</param>
        /// <param name="updateBreweryRequest">The updated brewery data.</param>
        /// <returns>An HTTP OK response with a success message if the record is updated successfully. 
        /// A BadRequest response if the data is invalid or missing. A NotFound response if no brewery record exists with the specified ID.</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBrewery(int id, CreateBreweryRequest updateBreweryRequest)
        {
            if (!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid brewery ID.", StatusCodes.Status400BadRequest));

            if (updateBreweryRequest == null)
                return BadRequest(ErrorResponse("Brewery data parameter cannot be null.", StatusCodes.Status400BadRequest));
            
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse("Brewery data passed is missing or invalid. Please check the data.", StatusCodes.Status400BadRequest));

            var result = await _breweryProcess.UpdateBrewery(id, _mapper.Map<CreateBreweryModel>(updateBreweryRequest));
            if (!result)
                return NotFound(ErrorResponse($"Brewery with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(new { message = "Brewery updated successfully." });
        }

        /// <summary>Deletes a brewery record by its ID.</summary>
        /// <param name="id">The ID of the brewery to delete. Must be greater than 0.</param>
        /// <returns>An HTTP OK response with a success message if the record is deleted successfully. 
        /// A BadRequest response if the brewery ID is invalid. A NotFound response if no brewery record exists with the specified ID.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBrewery(int id)
        {
            if (!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid brewery ID.", StatusCodes.Status400BadRequest));

            var result = await _breweryProcess.DeleteBrewery(id);
            if (!result)
                return NotFound(ErrorResponse($"Brewery with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(new { message = "Brewery deleted successfully." });
        }

        /// <summary>Assigns a beer to a brewery using many-to-many relationship.</summary>
        /// <param name="breweryBeerRequest">An object containing the brewery ID and beer ID to be linked.</param>
        /// <returns>Returns an HTTP OK response with a success message, a NotFound response if no matching records exist, or a BadRequest response if any ID is invalid.</returns>
        [HttpPost("beer")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignBeerToBrewery(BreweryBeerRequest breweryBeerRequest)
        {
            if (breweryBeerRequest == null)
                return BadRequest(ErrorResponse("BreweryBeerRequest data cannot be null.", StatusCodes.Status400BadRequest));

            if (!IsValidId(breweryBeerRequest.BreweryId) || !IsValidId(breweryBeerRequest.BeerId))
                return BadRequest(ErrorResponse("Invalid BreweryId or BeerId.", StatusCodes.Status400BadRequest));

            var result = await _breweryProcess.AssignBeerToBrewery(_mapper.Map<BreweryBeerModel>(breweryBeerRequest));
            
            return result switch
            {
                AssignmentResult.Success => Ok(new { message = "Beer assigned to brewery successfully." }),
                AssignmentResult.AlreadyExists => Ok(new { message = "Beer already assigned to brewery." }),
                AssignmentResult.NotFound => NotFound(ErrorResponse($"Brewery with ID {breweryBeerRequest.BreweryId} or Beer with ID {breweryBeerRequest.BeerId} not found.", StatusCodes.Status404NotFound)),
                _ => BadRequest(ErrorResponse("Unknown error occurred.", StatusCodes.Status400BadRequest))
            };
        }
    }
}
