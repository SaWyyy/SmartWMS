using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services;
using SmartWMS.Services.Interfaces;
using SmartWMS.SignalR;

namespace SmartWMSTests.Services;

public class InventoryStatusServiceTest
{
    private readonly IProductRepository _repository;
    private readonly IAlertRepository _alertRepository;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IInventoryStatusService _service;

    public InventoryStatusServiceTest()
    {
        this._repository = A.Fake<IProductRepository>();
        this._alertRepository = A.Fake<IAlertRepository>();
        this._hubContext = A.Fake<IHubContext<NotificationHub>>();
        this._service = new InventoryStatusService(_repository, _alertRepository, _hubContext);

    }

    [Fact]
    public async void CheckInventory_ProductQuantityOk_ReturnsFalse()
    {
        // Arrange
        var productList = new List<ProductDto>
        {
            new ProductDto { Barcode = "Test", Price = 1, ProductName = "Test", Quantity = 10 },
            new ProductDto { Barcode = "Test2", Price = 1, ProductName = "Test2", Quantity = 5 }
        };

        A.CallTo(() => _repository.GetAll()).Returns(productList);

        // Act
        var result = await _service.CheckInventory();
        
        // Assert
        result.Should().Be(false);
    }
    
    [Fact]
    public async void CheckInventory_ProductQuantityLow_ReturnsTrue()
    {
        // Arrange
        var productList = new List<ProductDto>
        {
            new ProductDto { Barcode = "Test", Price = 1, ProductName = "Test", Quantity = 2 },
            new ProductDto { Barcode = "Test2", Price = 1, ProductName = "Test2", Quantity = 1 }
        };

        A.CallTo(() => _repository.GetAll()).Returns(productList);

        // Act
        var result = await _service.CheckInventory();
        
        // Assert
        result.Should().Be(true);
    }
}