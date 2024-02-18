using Core.Models.DTOs.Order;
// using Core.Services.Order;
// using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.Utilities;
// using Presentation.Controllers;
using Xunit.Abstractions;

public class OrderServiceTest
{
    private readonly OrderService _sut; //system unit test
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<ILogger<OrderService>> _logger = new();
    private readonly Mock<ILogger<OrdersController>> _loggerController = new();

    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(
            new Dictionary<string, string>
            {
                { "Kafka:Broker", "placeholder" },
                { "Kafka:Topic", "placeholder" },
                { "Kafka:GroupId", "placeholder" }
            }!)
        .Build();

    private readonly ITestOutputHelper output;


    public OrderServiceTest(ITestOutputHelper output)
    {
        _sut = new OrderService(_logger.Object, _orderRepoMock.Object, _configuration);
        this.output = output;
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrders_WhereOrdersExists()
    {
        // Arrage
        // order 1
        var order1Id = Guid.NewGuid();
        ICollection<Item> shoppingBasket1Items = new List<Item>()
        {
            new Item()
            {
                itemId = Guid.NewGuid(),
                offeringId = Guid.NewGuid(),
                orderId = order1Id,
                quantity = 5,
                totalPrice = 500
            }
        };
        
        // Arrange
        var order1CustomerId = Guid.NewGuid();
        var order1OrderDate = DateOnly.FromDateTime(DateTime.Now);
        var order1Status = OrderStatus.InProcess;
        float order1TotalPrice = 500;
        ICollection<Item> order1Items = shoppingBasket1Items;
        
        var order1 = new Order()
        {
            OrderId = order1Id,
            CustomerId = order1CustomerId,
            OrderDate = order1OrderDate,
            OrderStatus = order1Status,
            TotalPrice = order1TotalPrice,
            Items = order1Items
        };

        var order2Id = Guid.NewGuid();
        ICollection<Item> shoppingBasket2Items = new List<Item>()
        {
            new Item()
            {
                itemId = Guid.NewGuid(),
                offeringId = Guid.NewGuid(),
                orderId = order2Id,
                quantity = 5,
                totalPrice = 500
            }
        };
        
        // Arrange
        var order2CustomerId = Guid.NewGuid();
        var order2OrderDate = DateOnly.FromDateTime(DateTime.Now);
        var order2Status = OrderStatus.InProcess;
        float order2TotalPrice = 500;
        ICollection<Item> order2Items = shoppingBasket2Items;
        
        var order2 = new Order()
        {
            OrderId = order2Id,
            CustomerId = order2CustomerId,
            OrderDate = order2OrderDate,
            OrderStatus = order2Status,
            TotalPrice = order2TotalPrice,
            Items = order2Items
        };

        var ordersList = new List<Order>() { };
        ordersList.Add(order1);
        ordersList.Add(order2);

        _orderRepoMock.Setup(x => x.GetAllOrders())
            .ReturnsAsync(ordersList);

        // Act
        var orders = await _sut.GetOrders();

        // Assert
        // Test order 1
        order1Id.Should().Be(orders[0].OrderId.ToString());
        order1CustomerId.Should().Be(orders[0].CustomerId.ToString());
        order1OrderDate.Should().Be(orders[0].OrderDate);
        order1TotalPrice.Should().Be(orders[0].TotalPrice);
        order1Status.Should().Be(orders[0].OrderStatus);
        // Test order 2
        order2Id.Should().Be(orders[1].OrderId.ToString());
        order2CustomerId.Should().Be(orders[1].CustomerId.ToString());
        order2OrderDate.Should().Be(orders[1].OrderDate);
        order2TotalPrice.Should().Be(orders[1].TotalPrice);
        order2Status.Should().Be(orders[1].OrderStatus);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnNothing_WhenNoOrdersExists()
    {
        // Arrage
        var ordersList = new List<Order>() { };

        _orderRepoMock.Setup(x => x.GetAllOrders())
            .ReturnsAsync(() => null);

        // Act
        var orders = await _sut.GetOrders();
        Console.WriteLine(orders);
        // Assert
        orders.Should().BeNull();
    }

    [Fact]
    public async Task UpdateOrder_ShouldUpdateOrder_WhenOrderExists()
    {
        // Arrage
        // order 1
        var order1Id = Guid.NewGuid();
        ICollection<Item> shoppingBasket1Items = new List<Item>()
        {
            new Item()
            {
                itemId = Guid.NewGuid(),
                offeringId = Guid.NewGuid(),
                orderId = order1Id,
                quantity = 5,
                totalPrice = 500
            }
        };
        
        // Arrange
        var order1CustomerId = Guid.NewGuid();
        var order1OrderDate = DateOnly.FromDateTime(DateTime.Now);
        var order1Status = OrderStatus.InProcess;
        float order1TotalPrice = 500;
        ICollection<Item> order1Items = shoppingBasket1Items;
        
        var order1 = new Order()
        {
            OrderId = order1Id,
            CustomerId = order1CustomerId,
            OrderDate = order1OrderDate,
            OrderStatus = order1Status,
            TotalPrice = order1TotalPrice,
            Items = order1Items
        };
        
        var orderUpdateDto = new OrderUpdateDto()
        {
            OrderStatus = OrderStatus.Completed
        };
    
        _orderRepoMock.Setup(x => x.GetOrder(order1.OrderId)).ReturnsAsync(order1);
        _orderRepoMock.Setup(x => x.UpdateOrder(order1)).Returns(Task.CompletedTask);
    
        // Act
        await _sut.UpdateOrder(order1.OrderId, orderUpdateDto);
    
        // Assert
        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) =>
                    @object.ToString() == $"Order was updated ${order1.OrderId}"
                    && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateOrder_ShouldNotUpdateOrder_UpdatedOrderIsNotFound()
    {
        // Arrage
        var Guid1 = Guid.NewGuid();
        var Guid2 = Guid.NewGuid();
    
        // Arrage
        // order 1
        var order1Id = Guid1;
        ICollection<Item> shoppingBasket1Items = new List<Item>()
        {
            new Item()
            {
                itemId = Guid.NewGuid(),
                offeringId = Guid.NewGuid(),
                orderId = order1Id,
                quantity = 5,
                totalPrice = 500,
                itemState = "active",
                shoppingBasketId = Guid.NewGuid()
            }
        };
        
        // Arrange
        var order1CustomerId = Guid.NewGuid();
        var order1OrderDate = DateOnly.FromDateTime(DateTime.Now);
        var order1Status = OrderStatus.InProcess;
        float order1TotalPrice = 500;
        ICollection<Item> order1Items = shoppingBasket1Items;
        
        var order1 = new Order()
        {
            OrderId = order1Id,
            CustomerId = order1CustomerId,
            OrderDate = order1OrderDate,
            OrderStatus = order1Status,
            TotalPrice = order1TotalPrice,
            Items = order1Items,
        };
        
        var orderUpdateDto = new OrderUpdateDto()
        {
            OrderStatus = OrderStatus.Completed
        };
    
        _orderRepoMock.Setup(x => x.GetOrder(order1.OrderId)).ReturnsAsync(order1);
        _orderRepoMock.Setup(x => x.UpdateOrder(order1)).Returns(Task.CompletedTask);
    
        // Act
        await _sut.UpdateOrder(Guid2, orderUpdateDto);
    
        // Assert
        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) =>
                    @object.ToString() == $"Order not found"
                    && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateOrder_ShouldReturnCorrectOrderUpdateDto_WhenOrderIsInDatabase()
    {
        // Arrage
        // order 1
        var order1Id = Guid.NewGuid();
        ICollection<Item> shoppingBasket1Items = new List<Item>()
        {
            new Item()
            {
                itemId = Guid.NewGuid(),
                offeringId = Guid.NewGuid(),
                orderId = order1Id,
                quantity = 5,
                totalPrice = 500
            }
        };
        
        // Arrange
        var order1CustomerId = Guid.NewGuid();
        var order1OrderDate = DateOnly.FromDateTime(DateTime.Now);
        var order1Status = OrderStatus.InProcess;
        float order1TotalPrice = 500;
        ICollection<Item> order1Items = shoppingBasket1Items;
        
        var order1 = new Order()
        {
            OrderId = order1Id,
            CustomerId = order1CustomerId,
            OrderDate = order1OrderDate,
            OrderStatus = order1Status,
            TotalPrice = order1TotalPrice,
            Items = order1Items
        };
    
        var orderUpdateDto = new OrderUpdateDto()
        {
            OrderStatus = OrderStatus.Completed
        };
    
        _orderRepoMock.Setup(x => x.GetOrder(order1.OrderId)).ReturnsAsync(order1);
        _orderRepoMock.Setup(x => x.UpdateOrder(order1)).Returns(Task.CompletedTask);
    
        // Act
        await _sut.UpdateOrder(order1.OrderId, orderUpdateDto);
        
        var returnedOrder = await _sut.GetOrder(order1.OrderId);
    
        // Assert
        returnedOrder.OrderStatus.Should().Be(orderUpdateDto.OrderStatus); // .ToString()
    }
}