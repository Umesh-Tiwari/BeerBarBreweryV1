using AutoMapper;
using Business.BeerBarBrewery.Process.Interface;
using Contract.BeerBarBrewery;
using Microsoft.AspNetCore.Mvc;
using Model.BeerBarBrewery;

namespace BeerBarBrewery.Controllers
{
    /// <summary>
    /// The BeerController is a web API controller class that handles all HTTP requests related to beer operations in the application
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BeerController : BaseController
    {
        /// <summary>
        ///  IBeerProcess instance holds a reference to a service responsible for handling beer-related business logic
        /// </summary>
        private readonly IBeerProcess _beerProcess;
        /// <summary>
        /// IMapper is an interface from the AutoMapper library used to automatically map object to object. 
        /// Here it is used to map beer contract with model and model with beer contract
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// BeerController Constructor injecting business logic layer and AutoMapper
        /// </summary>
        /// <param name="beerProcess"> Service object to carry out business process </param>
        /// <param name="mapper"> Mapper used to map object to object. here it is used to map contract object to model object</param>
        public BeerController(IBeerProcess beerProcess, IMapper mapper)
        {
            _beerProcess = beerProcess;
            _mapper = mapper;
        }

        /// <summary>
        /// The GetAllBeers method will return all the beer records. In case there is no record it will return not found with error response message.
        /// </summary>
        /// <returns>if record is present it will return List of beer details with OK response and return NotFound response when there is no records present</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BeerResponse>>> GetAllBeers()
        {
            var beerModels = await _beerProcess.GetAllBeers();
            if(!beerModels.Any())
                return NotFound(ErrorResponse("Beer data not found.", StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<IEnumerable<BeerResponse>>(beerModels));
        }

        /// <summary>
        /// Retrieves a specific beer by its ID.
        /// </summary>
        /// <param name="id">The ID of the beer against which the beer record will be displayed.</param>
        /// <returns>returns BeerResponse object with HTTP Ok response if the record exists  
        /// or return NotFound response(ErrorResponse) when there are no records present 
        /// or will return bad request if id is invalid. Valid id should be greater than 0 </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BeerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BeerResponse>> GetBeerById(int id)
        {
            if (!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid beer ID.", StatusCodes.Status400BadRequest));

            var beer = await _beerProcess.GetBeerById(id);
            if (beer == null)
                return NotFound(ErrorResponse($"Beer with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<BeerResponse>(beer));
        }

        /// <summary>
        /// Retrieves beers within a specified alcohol by volume (ABV) range. The range should be between parameter gtAlcoholByVolume and ltAlcoholByVolume
        /// </summary>
        /// <param name="gtAlcoholByVolume">The minimum ABV (greater than).</param>
        /// <param name="ltAlcoholByVolume">The maximum ABV (less than).</param>
        /// <returns>Returns all the beer record specified alcohol by volume (ABV) range along with HTTP OK response
        /// returns bad request if invalid alcohol volume values are passed
        /// returns not found in case there is no record found in specified alcohol by volume (ABV) range</returns>
        [ProducesResponseType(typeof(IEnumerable<BeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [HttpGet("BeerByRange")]
        public async Task<ActionResult<IEnumerable<BeerResponse>>> GetBeersByAlcoholVolumeRange(double gtAlcoholByVolume, double ltAlcoholByVolume)
        {
            if (gtAlcoholByVolume < 0 || ltAlcoholByVolume < 0)
                return BadRequest(ErrorResponse("Alcohol content values must be greater than or equal to 0.",StatusCodes.Status400BadRequest));

            if (gtAlcoholByVolume >= ltAlcoholByVolume)
                return BadRequest(ErrorResponse("Minimum alcohol volume must be less than maximum.",StatusCodes.Status400BadRequest));

            var beerList = await _beerProcess.GetBeersByAlcoholVolumeRange(gtAlcoholByVolume, ltAlcoholByVolume);
            if(!beerList.Any())
            {
                return NotFound(ErrorResponse("No beer records found with the above range of alcohol by volume.", StatusCodes.Status404NotFound));
            }
            
            return Ok(_mapper.Map<IEnumerable<BeerResponse>>(beerList));
        }

        /// <summary>
        /// This method will create new beer record based on the beer details passed. The brewery id is optional and can be linked later.
        /// </summary>
        /// <param name="createBeerRequest">The beer data to create.</param>
        /// <returns>It will return HTTP create response if the record is created successfully
        /// It will return bad request in case parameter passed is null or invalid</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BeerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BeerResponse>> CreateBeer(CreateBeerRequest createBeerRequest)
        {
            if (createBeerRequest == null)
                return BadRequest(ErrorResponse("Beer data passed as parameter is null.", StatusCodes.Status400BadRequest));

            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse("Beer data passed is missing or invalid. Please check the data.", StatusCodes.Status400BadRequest));

            var beerModel = await _beerProcess.CreateBeer(_mapper.Map<CreateBeerModel>(createBeerRequest));
            var beerResponse = _mapper.Map<BeerResponse>(beerModel);

            return CreatedAtAction(nameof(GetBeerById), new { id = beerModel.Id }, beerResponse);
        }

        /// <summary>
        /// Updates an existing beer by ID with the modified beer data.
        /// </summary>
        /// <param name="id">The ID of the beer to update.</param>
        /// <param name="updateBeerRequest">The updated beer data.</param>
        /// <returns>The string message with HTTP Ok response if the record is updated successfully 
        /// returns bad request if the data is invalid or missing
        /// returns not found response if the record does not exist </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBeer(int id, CreateBeerRequest updateBeerRequest)
        {
            if(!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid beer ID.", StatusCodes.Status400BadRequest));

            if (updateBeerRequest == null)
                return BadRequest(ErrorResponse("Beer data parameter cannot be null.", StatusCodes.Status400BadRequest));
            
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse("Invalid beer data.", StatusCodes.Status400BadRequest));

            var result = await _beerProcess.UpdateBeer(id, _mapper.Map<CreateBeerModel>(updateBeerRequest));
            if (!result)
                return NotFound(ErrorResponse($"Beer with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(new { message = "Beer updated successfully." });
        }

        /// <summary>
        /// Deletes a beer by its ID.
        /// </summary>
        /// <param name="id">The ID of the beer to delete.</param>
        /// <returns>The string message with Ok response if the record is deleted successfully 
        /// returns bad request if the beer id is invalid.
        /// returns not found response if the record does not exist </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBeer(int id)
        {
            if (!IsValidId(id))
                return BadRequest(ErrorResponse("Invalid beer ID.", StatusCodes.Status400BadRequest));

            var result = await _beerProcess.DeleteBeer(id);
            if (!result)
                return NotFound(ErrorResponse($"Beer with ID {id} not found.", StatusCodes.Status404NotFound));

            return Ok(new { message = "Beer deleted successfully." });
        }
    }
}
