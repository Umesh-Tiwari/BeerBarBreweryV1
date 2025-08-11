using AutoMapper;
using Contract.BeerBarBrewery;
using Model.BeerBarBrewery;

namespace BeerBarBrewery.Mapping
{
    /// <summary>
    /// AutoMapper profile for mapping between API contracts (DTOs) and domain models.
    /// Supports bidirectional mapping for all entity types in the beer bar brewery system.
    /// </summary>
    public class ContractToModelMapping : Profile
    {
        /// <summary>
        /// Configures all contract-to-model mappings for Bar, Beer, and Brewery entities.
        /// Uses convention-based mapping with ReverseMap for bidirectional support.
        /// </summary>
        public ContractToModelMapping()
        {
            CreateMap<BarWithBeerResponse, BarModel>().ReverseMap();
            CreateMap<BarResponse, BarModel>().ReverseMap();
            CreateMap<CreateBarRequest, CreateBarModel>().ReverseMap();

            CreateMap<BeerResponse, BeerModel>().ReverseMap();
            CreateMap<CreateBeerRequest, CreateBeerModel>().ReverseMap();

            CreateMap<BreweryResponse, BreweryModel>().ReverseMap();
            CreateMap<BreweryWithBeerResponse, BreweryModel>().ReverseMap();
            CreateMap<CreateBreweryRequest, CreateBreweryModel>().ReverseMap();
            CreateMap<BreweryWithBeerRequest, BreweryBeerModel>().ReverseMap();
            CreateMap<BreweryBeerRequest, BreweryBeerModel>().ReverseMap();
        }
    }
}
