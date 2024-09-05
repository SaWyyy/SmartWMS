using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;

namespace SmartWMS.Repositories;

public class ProductDetailRepository : IProductDetailRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProductDetailRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<ProductDetail> Add(ProductDetailDto dto)
    {
        var productDetail = _mapper.Map<ProductDetail>(dto);
        await _dbContext.ProductDetails.AddAsync(productDetail);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return productDetail;

        throw new Exception("Error has occured while saving changes");
    }

    public async Task<IEnumerable<ProductDetailDto>> GetAll()
    {
        var result = await _dbContext.ProductDetails.ToListAsync();
        var productDetails = _mapper.Map<IEnumerable<ProductDetailDto>>(result);
        return productDetails;
    }

    public async Task<ProductDetailDto> Get(int id)
    {
        var productDetail = await _dbContext.ProductDetails.FirstOrDefaultAsync(r => r.ProductDetailId == id);

        if (productDetail is null)
            throw new Exception("ProductDetail with specified id hasn't been found");
        var productDetailDto = _mapper.Map<ProductDetailDto>(productDetail);
        return productDetailDto;
    }

    public async Task<ProductDetail> Delete(int id)
    {
        var productDetail = await _dbContext.ProductDetails.FirstOrDefaultAsync(r => r.ProductDetailId == id);
        
        if (productDetail is null)
            throw new Exception("Product Detail with specified id hasn't been found");

        _dbContext.Remove(productDetail);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return productDetail;

        throw new Exception("Error has occured while saving changes");
    }

    public async Task<ProductDetail> Update(int id, ProductDetailDto dto)
    {
        var productDetail = await _dbContext.ProductDetails.FirstOrDefaultAsync(r => r.ProductDetailId == id);

        if (productDetail is null)
            throw new Exception("Product Detail with specified id hasn't been found");

        productDetail.Barcode = dto.Barcode;
        productDetail.Quantity = dto.Quantity;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return productDetail;

        throw new Exception("Error has occured while saving changes");
    }
}