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
        /// Retrieves a specific beer by its ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the beer to retrieve. Must be greater than 0.
        /// </param>
        /// <returns>
        /// - An HTTP OK response containing a BeerResponse object if the record exists.  
        /// - A NotFound response with an ErrorResponse if no record is found.  
        /// - A BadRequest response if the ID is invalid (i.e., less than or equal to 0).
        /// </returns>
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
        /// Retrieves beers based on alcohol by volume (ABV) criteria.
        /// </summary>
        /// <param name="gtAlcoholByVolume">
        /// Optional minimum ABV value (exclusive). If provided alone, returns beers with ABV greater than this value.
        /// </param>
        /// <param name="ltAlcoholByVolume">
        /// Optional maximum ABV value (exclusive). If provided alone, returns beers with ABV less than this value.
        /// </param>
        /// <returns>
        /// - An HTTP OK response containing a list of beers matching the ABV criteria.  
        /// - A BadRequest response if the ABV values are invalid or both parameters are missing.  
        /// - A NotFound response if no beers are found matching the criteria.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BeerResponse>>> GetBeersByAlcoholVolumeRange(decimal? gtAlcoholByVolume = null, decimal? ltAlcoholByVolume = null)
        {
            if (!gtAlcoholByVolume.HasValue && !ltAlcoholByVolume.HasValue)
                return BadRequest(ErrorResponse("At least one alcohol volume parameter must be provided.", StatusCodes.Status400BadRequest));

            if (gtAlcoholByVolume.HasValue && gtAlcoholByVolume < 0)
                return BadRequest(ErrorResponse("Minimum alcohol volume must be greater than or equal to 0.", StatusCodes.Status400BadRequest));

            if (ltAlcoholByVolume.HasValue && ltAlcoholByVolume < 0)
                return BadRequest(ErrorResponse("Maximum alcohol volume must be greater than or equal to 0.", StatusCodes.Status400BadRequest));

            if (gtAlcoholByVolume.HasValue && ltAlcoholByVolume.HasValue && gtAlcoholByVolume >= ltAlcoholByVolume)
                return BadRequest(ErrorResponse("Minimum alcohol volume must be less than maximum.", StatusCodes.Status400BadRequest));

            var beerList = await _beerProcess.GetBeersByAlcoholVolumeRange(gtAlcoholByVolume, ltAlcoholByVolume);
            if (!beerList.Any())
            {
                return NotFound(ErrorResponse("No beer records found with the specified alcohol volume criteria.", StatusCodes.Status404NotFound));
            }

            return Ok(_mapper.Map<IEnumerable<BeerResponse>>(beerList));
        }

        /// <summary>Creates a new beer record using the provided beer details.  
        /// The brewery ID is optional and can be linked later.
        /// </summary>
        /// <param name="createBeerRequest">The beer data used to create the new record.</param>
        /// <returns>
        /// - An HTTP Created response if the beer record is successfully created.  
        /// - A BadRequest response if the input data is null or invalid.
        /// </returns>
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
        /// Updates an existing beer record by its ID using the provided data.
        /// </summary>
        /// <param name="id">The ID of the beer to update. Must be greater than 0.</param>
        /// <param name="updateBeerRequest">The updated beer data.</param>
        /// <returns>
        /// - An HTTP OK response with a success message if the record is updated successfully.  
        /// - A BadRequest response if the input data is invalid or missing.  
        /// - A NotFound response if no beer record exists with the specified ID.
        /// </returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
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

        /// <summary>Deletes a beer record by its ID.</summary>
        /// <param name="id">The ID of the beer to delete. Must be greater than 0.</param>
        /// <returns>
        /// - An HTTP OK response with a success message if the record is deleted successfully.  
        /// - A BadRequest response if the beer ID is invalid.  
        /// - A NotFound response if no beer record exists with the specified ID.
        /// </returns>
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
