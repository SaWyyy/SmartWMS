using AutoMapper;
using SmartWMS.Controllers;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using Task = SmartWMS.Entities.Task;

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
        CreateMap<Alert, AlertDto>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Report, ReportDto>().ReverseMap();
    }
}