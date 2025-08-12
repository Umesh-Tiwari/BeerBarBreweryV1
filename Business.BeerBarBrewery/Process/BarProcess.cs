using AutoMapper;
using Business.BeerBarBrewery.Process.Interface;
using Contract.BeerBarBrewery;
using Database.BeerBarBrewery.Repository.Interface;
using Database.Entities;
using Model.BeerBarBrewery;

namespace Business.BeerBarBrewery.Process
{
    /// <summary>
    /// Implements business logic for managing bars and their relationship with beers.
    /// </summary>
    public class BarProcess : IBarProcess
    {
        private readonly IBarRepository _barRepository;
        private readonly IBeerRepository _beerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor to inject required repositories and AutoMapper.
        /// </summary>
        public BarProcess(IBarRepository barRepository, IBeerRepository beerRepository, IMapper mapper)
        {
            _barRepository = barRepository;
            _beerRepository = beerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Associates a beer with a specific bar.
        /// </summary>
        /// <param name="barBeerLinkDto">DTO containing bar and beer IDs to link.</param>
        /// <returns>AssignmentResult indicating success, already exists, or not found.</returns>
        public async Task<AssignmentResult> LinkBarToBeer(BarBeerRequest barBeerLinkDto)
        {
            var bar = await _barRepository.GetByIdAsync(barBeerLinkDto.BarId);
            var beer = await _beerRepository.GetByIdAsync(barBeerLinkDto.BeerId);

            if (bar == null || beer == null)
                return AssignmentResult.NotFound;

            var isNewRelationship = await _barRepository.AssignBeerAsync(barBeerLinkDto.BarId, barBeerLinkDto.BeerId);
            await _barRepository.SaveChangesAsync();

            return isNewRelationship ? AssignmentResult.Success : AssignmentResult.AlreadyExists;
        }

        /// <summary>
        /// Retrieves all beers served at a given bar.
        /// </summary>
        /// <param name="barId">ID of the bar.</param>
        /// <returns>List of beers served at the bar; empty list if none.</returns>
        public async Task<IEnumerable<BeerModel>> GetBeersServedAtBarAsync(int barId)
        {
            var beerList = await _barRepository.GetBeersServedAtBarAsync(barId);
            if (beerList == null)
                return Enumerable.Empty<BeerModel>();

            return _mapper.Map<IEnumerable<BeerModel>>(beerList);
        }

        /// <summary>
        /// Retrieves all bars without their associated beers.
        /// </summary>
        /// <returns>List of bars.</returns>
        public async Task<IEnumerable<BarModel>> GetAllBars()
        {
            var bars = await _barRepository.GetAllAsync();
            if (bars == null)
                return Enumerable.Empty<BarModel>();
            return _mapper.Map<IEnumerable<BarModel>>(bars);
        }

        /// <summary>
        /// Retrieves all bars along with the beers they serve.
        /// </summary>
        /// <returns>List of bars with their associated beers.</returns>
        public async Task<IEnumerable<BarModel>> GetAllBarsWithBeers()
        {
            var bars = await _barRepository.GetAllBarsWithBeersAsync();
            if (bars == null)
                return Enumerable.Empty<BarModel>();
            return _mapper.Map<IEnumerable<BarModel>>(bars);
        }

        /// <summary>
        /// Retrieves a specific bar by its ID.
        /// </summary>
        /// <param name="id">ID of the bar.</param>
        /// <returns>Bar model if found; otherwise null.</returns>
        public async Task<BarModel> GetBarById(int id)
        {
            var bar = await _barRepository.GetByIdAsync(id);
            if (bar == null)
                return null;

            return _mapper.Map<BarModel>(bar);
        }

        /// <summary>
        /// Creates a new bar.
        /// </summary>
        /// <param name="createBarModel">Model containing bar creation data.</param>
        /// <returns>The created bar model.</returns>
        public async Task<BarModel> CreateBar(CreateBarModel createBarModel)
        {
            var barEntity = _mapper.Map<Bar>(createBarModel);
            await _barRepository.AddAsync(barEntity);
            await _barRepository.SaveChangesAsync();

            return _mapper.Map<BarModel>(barEntity);
        }

        /// <summary>
        /// Updates an existing bar identified by ID.
        /// </summary>
        /// <param name="id">ID of the bar to update.</param>
        /// <param name="updateBarModel">Model with updated bar data.</param>
        /// <returns>True if update is successful; false if bar not found.</returns>
        public async Task<bool> UpdateBar(int id, CreateBarModel updateBarModel)
        {
            var barEntity = await _barRepository.GetByIdAsync(id);
            if (barEntity == null)
                return false;

            _mapper.Map(updateBarModel, barEntity);
            _barRepository.Update(barEntity);
            await _barRepository.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Deletes a bar by its ID.
        /// </summary>
        /// <param name="id">ID of the bar to delete.</param>
        /// <returns>True if deletion is successful; false if bar not found.</returns>
        public async Task<bool> DeleteBar(int id)
        {
            var bar = await _barRepository.GetByIdAsync(id);
            if (bar == null)
                return false;

            _barRepository.Delete(bar);
            await _barRepository.SaveChangesAsync();
            return true;
        }
    }
}
