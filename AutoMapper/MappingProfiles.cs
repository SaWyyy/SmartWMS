using AutoMapper;
using SmartWMS.Controllers;
using SmartWMS.Models;
using Task = SmartWMS.Models.Task;

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
        CreateMap<Task, TaskDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Subcategory, SubcategoryDto>().ReverseMap();
        CreateMap<ProductDetail, ProductDetailDto>().ReverseMap();
    }
}