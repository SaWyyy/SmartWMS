using AutoMapper;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.CreateOrderDtos;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services.Interfaces;

namespace SmartWMS.Services;

public class OrderAndTasksCreationService : IOrderAndTasksCreationService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderHeaderRepository _orderHeaderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IWaybillRepository _waybillRepository;
    private readonly IMapper _mapper;
    
    public OrderAndTasksCreationService(
        IProductRepository productRepository,
        IOrderHeaderRepository orderHeaderRepository,
        IOrderDetailRepository orderDetailRepository,
        ITaskRepository taskRepository,
        ICountryRepository countryRepository,
        IWaybillRepository waybillRepository,
        IMapper mapper
        )
    {
        this._productRepository = productRepository;
        this._orderHeaderRepository = orderHeaderRepository;
        this._orderDetailRepository = orderDetailRepository;
        this._taskRepository = taskRepository;
        this._countryRepository = countryRepository;
        this._waybillRepository = waybillRepository;
        this._mapper = mapper;
    }
    
    public async Task CreateOrder(CreateOrderDto dto)
    {
        if (dto.Products.Any(x => x.ProductId == null))
            throw new SmartWMSExceptionHandler("Product id is null");
        try
        {
            var country = await _countryRepository.Get(dto.Waybill.CountryId);
            
            foreach (var product in dto.Products)
            {
                var dbProduct = await _productRepository.Get(product.ProductId);
                if (product.Quantity > dbProduct.Quantity)
                    throw new SmartWMSExceptionHandler(
                        "Product's quantity assigned to order exceeds number of pieces available at the warehouse");
            }

            var orderHeaderDto = new OrderHeaderDto
            {
                DestinationAddress = dto.OrderHeader.DestinationAddress,
                DeliveryDate = DateTime.Now,
                OrderDate = DateTime.Now,
                StatusName = OrderName.Planned,
                TypeName = OrderType.Shipment
            };

            var orderHeader = await _orderHeaderRepository.Add(orderHeaderDto);

            foreach (var product in dto.Products)
            {
                var orderDetailDto = new OrderDetailDto
                {
                    Quantity = product.Quantity,
                    ProductsProductId = product.ProductId,
                    OrderHeadersOrdersHeaderId = orderHeader.OrdersHeaderId
                };

                var orderDetail = await _orderDetailRepository.Add(orderDetailDto);

                var taskDto = new TaskDto
                {
                    OrderDetailsOrderDetailId = orderDetail.OrderDetailId,
                    Priority = 3,
                    StartDate = DateTime.Now,
                    QuantityAllocated = orderDetail.Quantity
                };

                await _taskRepository.AddTask(taskDto);

                var updateProduct = await _productRepository.Get(product.ProductId);
                updateProduct.Quantity -= product.Quantity;
                await _productRepository.Update(product.ProductId, updateProduct);
            }

            var waybillDto = new WaybillDto
            {
                Barcode = await GenerateBarcode(),
                CountriesCountryId = country.CountryId.GetValueOrDefault(),
                LoadingDate = DateTime.Now,
                ShippingDate = DateTime.Now,
                OrderHeadersOrderHeaderId = orderHeader.OrdersHeaderId,
                PostalCode = dto.Waybill.PostalCode,
                SupplierName = dto.Waybill.SupplierName
            };

            await _waybillRepository.AddWaybill(waybillDto);
        }
        catch (SmartWMSExceptionHandler e)
        {
            throw new SmartWMSExceptionHandler(e.Message);
        }
    }

    private async Task<string> GenerateBarcode()
    {
        var productsDtos = await _productRepository.GetAll();
        var products = productsDtos.ToList();

        var random = new Random();
        string ean13Code;
        do
        {
            string baseDigits = new string(Enumerable.Range(0, 12).Select(_ => random.Next(0, 10).ToString()[0]).ToArray());
            int checkDigit = CalculateEan13CheckDigit(baseDigits);
            ean13Code = baseDigits + checkDigit;

        } while (products.Any(product => product.Barcode == ean13Code));

        return ean13Code;
    }

    private int CalculateEan13CheckDigit(string baseDigits)
    {
        if (baseDigits.Length != 12 || !baseDigits.All(char.IsDigit))
            throw new ArgumentException("Podstawa kodu EAN-13 musi mieć dokładnie 12 cyfr.");

        int sum = 0;

        for (int i = 0; i < baseDigits.Length; i++)
        {
            int digit = int.Parse(baseDigits[i].ToString());
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        int checkDigit = (10 - (sum % 10)) % 10;

        return checkDigit;
    }
}