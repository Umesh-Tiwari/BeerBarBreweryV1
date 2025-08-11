using AutoMapper;
using Database.Entities;
using Model.BeerBarBrewery;

namespace Business.BeerBarBrewery.Mapping
{
    /// <summary>
    /// AutoMapper profile for mapping between business models and data entities.
    /// Used in the data access layer to translate between domain models and database entities.
    /// </summary>
    public class ModelToDataEntityMapping : Profile
    {
        /// <summary>
        /// Configures all model-to-database-entities mappings for Bar, Beer, and Brewery entities.
        /// Uses convention-based mapping with ReverseMap for bidirectional support.
        /// </summary>
        public ModelToDataEntityMapping()
        {
            //Bar mappings
            CreateMap<CreateBarModel, Bar>().ReverseMap();
            CreateMap<BarModel, Bar>().ReverseMap();
            CreateMap<Bar, BarModel>()
                .ForMember(dest => dest.Beers,
                           opt => opt.MapFrom(src => src.BarBeers.Select(bb => bb.Beer)));
            
            //Beer mappings
            CreateMap<BeerModel, Beer>().ReverseMap();
            CreateMap<CreateBeerModel, Beer>().ReverseMap();
            
            //Brewery mappings
            CreateMap<CreateBreweryModel, Brewery>().ReverseMap();
            CreateMap<BreweryModel, Brewery>()
                .ForMember(dest => dest.BreweryBeers, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Beers, opt => opt.MapFrom(src => src.BreweryBeers.Select(bb => bb.Beer)));
            
            //BreweryBeer mappings
            CreateMap<BreweryBeerModel, BreweryBeer>().ReverseMap();
            
        }
    }
}
