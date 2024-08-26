using AutoMapper;
using SmartWMS.Models;

namespace SmartWMS;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Shelf, ShelfDto>();
        CreateMap<Product, ProductDto>();
        CreateMap<CreateShelfDto, Shelf>();
    }
}