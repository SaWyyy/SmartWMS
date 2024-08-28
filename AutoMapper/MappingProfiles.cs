using AutoMapper;
using SmartWMS.Models;

namespace SmartWMS;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Shelf, ShelfDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<Waybill, WaybillDto>().ReverseMap();
        CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();

    }
}