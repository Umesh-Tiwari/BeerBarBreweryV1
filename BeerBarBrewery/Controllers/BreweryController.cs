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
        /// The GetAllBreweries method will return all the brewery records. In case there is no record it will return not found with error response message.
        /// </summary>
        /// <returns>If record is present it will return List of brewery details with OK response and return NotFound response when there are no records found</returns>
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

        /// <summary>
        /// The GetAllBreweriesWithBeer method will return brewery records along with beer records
        /// </summary>
        /// <returns>If record is present it will return List of brewery with beer details with OK response and return NotFound response when there are no records found</returns>
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

        /// <summary>
        /// The method will return brewery with beer details for specific brewery id.
        /// </summary>
        /// <param name="breweryId"> The brewery id for which the brewery with beer details to be displayed</param>
        /// <returns>Returns brewery with beer record associated with OK response,
        /// returns bad request if invalid brewery id is passed. The valid brewery id should be greater than 0,
        /// returns not found in case there is no brewery record</returns>
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

        /// <summary>
        /// This method adds a new brewery record
        /// </summary>
        /// <param name="createBreweryRequest">Brewery request object containing data to be saved</param>
        /// <returns>It will return HTTP create response if the record is created successfully.
        /// It will return bad request in case parameter passed is null or invalid</returns>
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

        /// <summary>
        /// Updates an existing brewery by ID with the modified brewery data.
        /// </summary>
        /// <param name="id">The ID of the brewery to update.</param>
        /// <param name="updateBreweryRequest">The updated brewery data.</param>
        /// <returns>The string message with HTTP Ok response if the record is updated successfully, 
        /// returns bad request if the data is invalid or missing,
        /// returns not found response if the record does not exist </returns>
        [HttpPut("{id}")]
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

        /// <summary>
        /// Deletes a brewery by its ID.
        /// </summary>
        /// <param name="id">The ID of the brewery to delete.</param>
        /// <returns>The string message with Ok response if the record is deleted successfully, 
        /// returns bad request if the brewery id is invalid. The valid id is greater than zero,
        /// returns not found response if the record does not exist </returns>
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

        /// <summary>
        /// This method will assign brewery to beer data. A single beer will be associated with one brewery 
        /// but a brewery can have many beer records.
        /// </summary>
        /// <param name="breweryWithBeerRequest"> This object will contain brewery id and beer id. The brewery id will be updated in beer record </param>
        /// <returns>Returns string message with HTTP Ok response
        /// or return NotFound response(ErrorResponse) when there are no records present 
        /// or will return bad request if id is invalid. Valid id should be greater than 0.
        /// </returns>
        [HttpPost("beer")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignBreweryToBeer(BreweryWithBeerRequest breweryWithBeerRequest)
        {
            if (breweryWithBeerRequest == null)
                return BadRequest(ErrorResponse("BreweryWithBeerRequest data cannot be null.", StatusCodes.Status400BadRequest));

            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse("Brewery data passed is missing or invalid. Please check the data.", StatusCodes.Status400BadRequest));

            if (!IsValidId(breweryWithBeerRequest.BreweryId) || !IsValidId(breweryWithBeerRequest.BeerId))
                return BadRequest(ErrorResponse("Invalid BreweryId or BeerId.", StatusCodes.Status400BadRequest));

            var result = await _breweryProcess.AssignBreweryToBeer(_mapper.Map<BreweryBeerModel>(breweryWithBeerRequest));
            if (!result)
                return NotFound(ErrorResponse($"Brewery with ID {breweryWithBeerRequest.BreweryId} or Beer with ID {breweryWithBeerRequest.BeerId} not found.", StatusCodes.Status404NotFound));

            return Ok(new { message = "Brewery assigned to beer successfully." });
        }
    }
}
