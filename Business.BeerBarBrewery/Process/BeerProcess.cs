using AutoMapper;
using Business.BeerBarBrewery.Process.Interface;
using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Model.BeerBarBrewery;

namespace Business.BeerBarBrewery.Process
{

    /// <summary>
    /// Implements business logic for managing beer entities.
    /// Handles creation, retrieval, update, deletion, and filtering of beers.
    /// </summary>
    public class BeerProcess : IBeerProcess
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor to inject repository and AutoMapper.
        /// </summary>
        public BeerProcess(IBeerRepository beerRepository, IMapper mapper)
        {
            _beerRepository = beerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all beers in the system.
        /// </summary>
        /// <returns>List of beer models.</returns>
        public async Task<IEnumerable<BeerModel>> GetAllBeers()
        {
            var beers = await _beerRepository.GetAllAsync();
            if (beers == null)
            {
                return Enumerable.Empty<BeerModel>();
            }
            return _mapper.Map<IEnumerable<BeerModel>>(beers);
        }

        /// <summary>
        /// Retrieves a specific beer by its ID.
        /// </summary>
        /// <param name="id">Beer ID to retrieve.</param>
        /// <returns>Beer model if found; otherwise null.</returns>
        public async Task<BeerModel> GetBeerById(int id)
        {
            var beer = await _beerRepository.GetByIdAsync(id);
            if (beer == null)
                return null;

            return _mapper.Map<BeerModel>(beer);
        }

        /// <summary>
        /// Creates a new beer using the provided model.
        /// </summary>
        /// <param name="createBeerModel">Model containing beer data.</param>
        /// <returns>The created beer model with assigned ID.</returns>
        public async Task<BeerModel> CreateBeer(CreateBeerModel createBeerModel)
        {
            var beer = _mapper.Map<Beer>(createBeerModel);
            await _beerRepository.AddAsync(beer);
            await _beerRepository.SaveChangesAsync();

            return _mapper.Map<BeerModel>(beer);
        }

        /// <summary>
        /// Updates an existing beer identified by ID.
        /// </summary>
        /// <param name="id">Beer ID to update.</param>
        /// <param name="updateModel">Model containing updated beer data.</param>
        /// <returns>True if update is successful; false if beer not found.</returns>
        public async Task<bool> UpdateBeer(int id, CreateBeerModel updateModel)
        {
            var beer = await _beerRepository.GetByIdAsync(id);
            if (beer == null)
                return false;

            _mapper.Map(updateModel, beer);
            _beerRepository.Update(beer);
            await _beerRepository.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves beers that fall within a specified alcohol by volume (ABV) range.
        /// </summary>
        /// <param name="minAbv">Minimum ABV (inclusive).</param>
        /// <param name="maxAbv">Maximum ABV (inclusive).</param>
        /// <returns>List of beers matching the ABV range.</returns>
        public async Task<IEnumerable<BeerModel>> GetBeersByAlcoholVolumeRange(double minAbv, double maxAbv)
        {
            var beerEntities = await _beerRepository.GetBeersByAlcoholVolumeRangeAsync(minAbv, maxAbv);
            if (beerEntities == null)
                return Enumerable.Empty<BeerModel>();
            return _mapper.Map<IEnumerable<BeerModel>>(beerEntities);
        }

        /// <summary>
        /// Deletes a beer by its ID.
        /// </summary>
        /// <param name="id">Beer ID to delete.</param>
        /// <returns>True if deletion is successful; false if beer not found.</returns>
        public async Task<bool> DeleteBeer(int id)
        {
            var beer = await _beerRepository.GetByIdAsync(id);
            if (beer == null)
                return false;

            _beerRepository.Delete(beer);
            await _beerRepository.SaveChangesAsync();
            return true;
        }
    }
}
