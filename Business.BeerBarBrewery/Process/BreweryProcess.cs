using AutoMapper;
using Business.BeerBarBrewery.Process.Interface;
using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Model.BeerBarBrewery;

namespace Business.BeerBarBrewery.Process
{
    /// <summary>
    /// Implements business logic for managing breweries and their association with beers.
    /// </summary>
    public class BreweryProcess : IBreweryProcess
    {
        private readonly IBreweryRepository _breweryRepository;
        private readonly IBeerRepository _beerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for injecting repositories and AutoMapper.
        /// </summary>
        public BreweryProcess(IBreweryRepository breweryRepository, IBeerRepository beerRepository, IMapper mapper)
        {
            _breweryRepository = breweryRepository;
            _beerRepository = beerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all breweries in the system.
        /// </summary>
        /// <returns>List of brewery models.</returns>
        public async Task<IEnumerable<BreweryModel>> GetAllBreweries()
        {
            var breweries = await _breweryRepository.GetAllAsync();
            if (breweries == null)
            {
                return Enumerable.Empty<BreweryModel>();
            }
            return _mapper.Map<IEnumerable<BreweryModel>>(breweries);
        }

        /// <summary>
        /// Retrieves all breweries along with the beers they produce.
        /// </summary>
        /// <returns>List of breweries with associated beers.</returns>
        public async Task<IEnumerable<BreweryModel>> GetAllWithBeerAsync()
        {
            var breweries = await _breweryRepository.GetAllWithBeerAsync();
            if (breweries == null)
            {
                return Enumerable.Empty<BreweryModel>();
            }
            return _mapper.Map<IEnumerable<BreweryModel>>(breweries);
        }

        /// <summary>
        /// Retrieves a specific brewery by ID.
        /// </summary>
        /// <param name="id">Brewery ID.</param>
        /// <returns>Brewery model if found; otherwise null.</returns>
        public async Task<BreweryModel> GetBreweryById(int id)
        {
            var brewery = await _breweryRepository.GetByIdAsync(id);
            if (brewery == null)
                return null;
            return _mapper.Map<BreweryModel>(brewery);
        }

        /// <summary>
        /// Creates a new brewery from the given model.
        /// </summary>
        /// <param name="createBreweryModel">Model containing brewery data.</param>
        /// <returns>The created brewery model with assigned ID.</returns>
        public async Task<BreweryModel> CreateBrewery(CreateBreweryModel createBreweryModel)
        {
            var brewery = _mapper.Map<Brewery>(createBreweryModel);
            await _breweryRepository.AddAsync(brewery);
            await _breweryRepository.SaveChangesAsync();

            return _mapper.Map<BreweryModel>(brewery);
        }

        /// <summary>
        /// Updates an existing brewery.
        /// </summary>
        /// <param name="id">ID of the brewery to update.</param>
        /// <param name="updateBreweryModel">Model with updated data.</param>
        /// <returns>True if update successful; false if brewery not found.</returns>
        public async Task<bool> UpdateBrewery(int id, CreateBreweryModel updateBreweryModel)
        {
            var existingBrewery = await _breweryRepository.GetByIdAsync(id);
            if (existingBrewery == null)
                return false;

            _mapper.Map(updateBreweryModel, existingBrewery);
            _breweryRepository.Update(existingBrewery);
            await _breweryRepository.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Associates a beer with a brewery by updating the beer's BreweryId.
        /// </summary>
        /// <param name="breweryBeerModel">Model containing BreweryId and BeerId.</param>
        /// <returns>True if association successful; false if beer or brewery not found.</returns>
        public async Task<bool> AssignBreweryToBeer(BreweryBeerModel breweryBeerModel)
        {
            var brewery = await _breweryRepository.GetByIdAsync(breweryBeerModel.BreweryId);
            var beer = await _beerRepository.GetByIdAsync(breweryBeerModel.BeerId);

            if (brewery == null || beer == null)
                return false;

            beer.BreweryId = brewery.Id;
            _beerRepository.Update(beer);
            await _beerRepository.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a brewery by its ID.
        /// </summary>
        /// <param name="id">ID of the brewery to delete.</param>
        /// <returns>True if deletion successful; false if brewery not found.</returns>
        public async Task<bool> DeleteBrewery(int id)
        {
            var brewery = await _breweryRepository.GetByIdAsync(id);
            if (brewery == null)
                return false;

            _breweryRepository.Delete(brewery);
            await _breweryRepository.SaveChangesAsync();
            return true;
        }
    }

}
