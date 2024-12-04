using AutoMapper;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.CreateOrderDtos;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services.Interfaces;

namespace SmartWMS.Services;

public class ProductAssignmentService : IProductAssignmentService
{
    private readonly IProductRepository _productRepository;
    private readonly IShelfRepository _shelfRepository;
    private readonly IMapper _mapper;

    public ProductAssignmentService(IProductRepository productRepository, IShelfRepository shelfRepository, IMapper mapper)
    {
        this._productRepository = productRepository;
        this._shelfRepository = shelfRepository;
        this._mapper = mapper;
    }
    
    public async Task CreateAndAssignProductToShelves(CreateProductAsssignShelfDto dto)
    {
        try
        {
            var productDto = _mapper.Map<ProductDto>(dto.ProductDto);
            var shelves = _mapper.Map<List<ShelfDto>>(dto.Shelves);

            if (productDto is null)
                throw new SmartWMSExceptionHandler("Product is null");
            
            if (!shelves.Any())
                throw new SmartWMSExceptionHandler("Sheves are empty");

            if (productDto.Quantity > shelves.Sum(x => x.MaxQuant))
                throw new SmartWMSExceptionHandler("Product quantity cannot exceed max quantity of shelves");
            
            if (!productDto.Quantity.Equals(shelves.Sum(x => x.CurrentQuant)))
                throw new SmartWMSExceptionHandler("Product quantity is not equal to summed current quantity in shelves");
            
            var product = await _productRepository.Add(productDto);
            
            foreach (var shelfDto in shelves)
            {
                int shelfId = shelfDto.ShelfId.GetValueOrDefault();
                shelfDto.ProductsProductId = product.ProductId;
                await _shelfRepository.Update(shelfId, shelfDto);
            }
        }
        catch (SmartWMSExceptionHandler e)
        {
            throw (new SmartWMSExceptionHandler(e.Message));
        }
    }

    public async Task AssignProductForDelivery(CreateProductAsssignShelfDto dto)
    {
        try
        {
            var productDto = _mapper.Map<ProductDto>(dto.ProductDto);
            var shelves = _mapper.Map<List<ShelfDto>>(dto.Shelves);

            if (productDto is null)
                throw new SmartWMSExceptionHandler("Product is null");

            if (!shelves.Any())
                throw new SmartWMSExceptionHandler("Sheves are empty");

            await _productRepository.UpdateQuantity(productDto);

            if (shelves.All(x => x.ProductsProductId != productDto.ProductId && x.ProductsProductId != null))
                throw new ConflictException("Trying to reassign product to shelf");
            
            foreach (var shelfDto in shelves)
            {
                if (shelfDto.ProductsProductId != productDto.ProductId && shelfDto.ProductsProductId == null)
                {
                    shelfDto.ProductsProductId = productDto.ProductId;
                }

                int shelfId = shelfDto.ShelfId.GetValueOrDefault();
                await _shelfRepository.Update(shelfId, shelfDto);
            }
        }
        catch (SmartWMSExceptionHandler e)
        {
            throw new SmartWMSExceptionHandler(e.Message);
        }
    }
}