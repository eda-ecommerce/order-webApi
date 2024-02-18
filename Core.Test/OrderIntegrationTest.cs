using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Models.DTOs.Order;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

public class OrderServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "Kafka:Broker", "placeholder" },
                    { "Kafka:Topic", "placeholder" },
                    { "Kafka:GroupId", "placeholder" }
                }!)
            .Build();

        public OrderServiceIntegrationTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
        }

        // [Fact]
        // public async Task GetOrders_ReturnsOrderDtoList()
        // {
        //     // Arrange
        //     var client = _factory.CreateClient();
        //
        //     // Mock the repository response
        //     var orderRepositoryMock = new Mock<IOrderRepository>();
        //     orderRepositoryMock.Setup(repo => repo.GetAllOrders())
        //         .ReturnsAsync(new List<Order>());
        //
        //     _factory.WithWebHostBuilder(builder =>
        //     {
        //         builder.ConfigureServices(services =>
        //         {
        //             services.AddSingleton(orderRepositoryMock.Object);
        //         });
        //     });
        //
        //     // Act
        //     var response = await client.GetAsync("/api/Orders");
        //     response.EnsureSuccessStatusCode();
        //
        //     var responseBody = await response.Content.ReadAsStringAsync();
        //
        //     var orderDtoList = JsonSerializer.Deserialize<List<Order>>(responseBody);
        //     
        //     // Assert
        //     Assert.NotNull(orderDtoList);
        // }
        //
        // // [Fact]
        // // public async Task GetOrder_WithValidId_ReturnsOrderDto()
        // // {
        // //     // Arrange
        // //     var dbContext = CreateDbContext();
        // //     
        // //     var client = _factory.CreateClient();
        // //     // Arrage
        // //     // order 1
        // //     var order1Id = Guid.NewGuid();
        // //     ICollection<Item> shoppingBasket1Items = new List<Item>()
        // //     {
        // //         new Item()
        // //         {
        // //             ItemId = Guid.NewGuid(),
        // //             OfferingId = Guid.NewGuid(),
        // //             OrderId = order1Id,
        // //             Quantity = 5,
        // //             TotalPrice = 500
        // //         }
        // //     };
        // //     
        // //     // Arrange
        // //     var order1CustomerId = Guid.NewGuid();
        // //     var order1OrderDate = DateOnly.FromDateTime(DateTime.Now);
        // //     var order1Status = OrderStatus.InProcess;
        // //     float order1TotalPrice = 500;
        // //     ICollection<Item> order1Items = shoppingBasket1Items;
        // //     
        // //     var order1 = new Order()
        // //     {
        // //         OrderId = order1Id,
        // //         CustomerId = order1CustomerId,
        // //         OrderDate = order1OrderDate,
        // //         OrderStatus = order1Status,
        // //         TotalPrice = order1TotalPrice,
        // //         Items = order1Items
        // //     };
        // //
        // //     // var ordersList = new Order() { };
        // //     // ordersList = order1;
        // //     
        // //     // If not already exists, than persist
        // //     await dbContext.Orders.AddAsync(order1);
        // //     await dbContext.SaveChangesAsync();
        // //
        // //     // Mock the repository response
        // //     var orderRepositoryMock = new Mock<IOrderRepository>();
        // //     orderRepositoryMock.Setup(repo => repo.GetOrder(order1Id))
        // //         .ReturnsAsync(order1);
        // //
        // //     _factory.WithWebHostBuilder(builder =>
        // //     {
        // //         builder.ConfigureServices(services =>
        // //         {
        // //             services.AddSingleton(orderRepositoryMock.Object);
        // //             
        // //         });
        // //     });
        // //
        // //     // Act
        // //     var response = await client.GetAsync($"/api/Orders/${order1Id}/Order");
        // //     response.EnsureSuccessStatusCode();
        // //
        // //     var responseBody = await response.Content.ReadAsStringAsync();
        // //     var orderDto = JsonSerializer.Deserialize<OrderDto>(responseBody);
        // //     _testOutputHelper.WriteLine(responseBody);
        // //     // Assert
        // //     Assert.NotNull(orderDto);
        // //     
        // //     // Test order 1
        // //     // order1Id.Should().Be(order.OrderId.ToString());
        // //     // order1CustomerId.Should().Be(order.CustomerId.ToString());
        // //     // order1OrderDate.Should().Be(order.OrderDate);
        // //     // order1TotalPrice.Should().Be(order.TotalPrice);
        // //     // order1Status.Should().Be(order.OrderStatus);
        // // }
        // [Fact]
        // public async Task GetOrder_WithValidId_ReturnsOrderDto()
        // {
        //     // Arrange
        //     var dbContext = CreateDbContext();
        //     var orderId = Guid.NewGuid();
        //
        //     var order1 = new Order
        //     {
        //         OrderId = orderId,
        //         // populate order properties
        //     };
        //
        //     
        //
        //     var orderRepositoryMock = new Mock<IOrderRepository>();
        //     orderRepositoryMock.Setup(repo => repo.GetOrder(orderId))
        //         .ReturnsAsync(order1);
        //
        //     _factory.WithWebHostBuilder(builder =>
        //     {
        //         builder.ConfigureServices(services =>
        //         {
        //             services.AddSingleton(orderRepositoryMock.Object);
        //         });
        //     });
        //
        //     var client = _factory.CreateClient();
        //     
        //     // Act
        //     var response1 = await client.GetAsync("/api/Orders");
        //     var responseBody1 = await response1.Content.ReadAsStringAsync();
        //     var orderDto1 = JsonSerializer.Deserialize<List<Order>>(responseBody1);
        //
        //     // Act
        //     if (orderDto1 != null)
        //     {   
        //         
        //         var response = await client.GetAsync($"/api/Orders/{orderDto1[0].OrderId}/Order");
        //         response.EnsureSuccessStatusCode();
        //
        //         var responseBody = await response.Content.ReadAsStringAsync();
        //         var orderDto = JsonSerializer.Deserialize<Order>(responseBody);
        //
        //         // Assert
        //         Assert.NotNull(orderDto);
        //     }
        //
        //     // Add more specific assertions based on your application logic
        // }
        //
        // private OrderDbContext CreateDbContext()
        // {
        //     var optins = new DbContextOptionsBuilder<OrderDbContext>()
        //         .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        //         .Options;
        //     var databaseContext = new OrderDbContext(optins);
        //     databaseContext.Database.EnsureCreated();
        //     return databaseContext;
        // }
        //
        // [Fact]
        // public async Task GetOrder_WithInvalidId_ReturnsNotFound()
        // {
        //     // Arrange
        //     var orderId = Guid.NewGuid();
        //     var client = _factory.CreateClient();
        //
        //     // Mock the repository response
        //     var orderRepositoryMock = new Mock<IOrderRepository>();
        //     orderRepositoryMock.Setup(repo => repo.GetOrder(orderId))
        //         .ReturnsAsync((Order)null!);
        //
        //     _factory.WithWebHostBuilder(builder =>
        //     {
        //         builder.ConfigureServices(services =>
        //         {
        //             services.AddSingleton(orderRepositoryMock.Object);
        //             // Add any other dependencies if needed
        //         });
        //     });
        //
        //     // Act
        //     var response = await client.GetAsync($"/api/Orders/{orderId}/Order");
        //     
        //     // Assert
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }
        //
        // [Fact]
        // public async Task UpdateOrder_WithValidData_ReturnsUpdatedOrderDto()
        // {
        //     // Arrange
        //     var orderId = Guid.NewGuid();
        //     var orderUpdateDto = new OrderUpdateDto
        //     {
        //         OrderStatus = OrderStatus.Cancelled
        //     };
        //     var client = _factory.CreateClient();
        //
        //     // Mock the repository response
        //     var orderRepositoryMock = new Mock<IOrderRepository>();
        //     orderRepositoryMock.Setup(repo => repo.GetOrder(orderId))
        //         .ReturnsAsync(new Order());
        //
        //     orderRepositoryMock.Setup(repo => repo.UpdateOrder(It.IsAny<Order>()));
        //
        //     _factory.WithWebHostBuilder(builder =>
        //     {
        //         builder.ConfigureServices(services =>
        //         {
        //             services.AddSingleton(orderRepositoryMock.Object);
        //             // Add any other dependencies if needed
        //         });
        //     });
        //
        //     // Act
        //     var response = await client.PutAsync($"/api/Orders/Update/{orderId}",
        //         new StringContent(JsonSerializer.Serialize(orderUpdateDto), Encoding.UTF8, "application/json"));
        //
        //     response.EnsureSuccessStatusCode();
        //
        //     var responseBody = await response.Content.ReadAsStringAsync();
        //     var updatedOrderDto = JsonSerializer.Deserialize<Order>(responseBody);
        //
        //     // Assert
        //     Assert.NotNull(updatedOrderDto);
        //     // Add more specific assertions based on your application logic
        // }
        //
        // [Fact]
        // public async Task UpdateOrder_WithInvalidId_ReturnsNotFound()
        // {
        //     // Arrange
        //     var orderId = Guid.NewGuid();
        //     var orderUpdateDto = new OrderUpdateDto
        //     {
        //         OrderStatus = OrderStatus.Completed
        //     };
        //     var client = _factory.CreateClient();
        //
        //     // Mock the repository response
        //     var orderRepositoryMock = new Mock<IOrderRepository>();
        //     orderRepositoryMock.Setup(repo => repo.GetOrder(orderId))
        //         .ReturnsAsync((Order)null!);
        //
        //     _factory.WithWebHostBuilder(builder =>
        //     {
        //         builder.ConfigureServices(services =>
        //         {
        //             services.AddSingleton(orderRepositoryMock.Object);
        //             services.AddSingleton(_configuration);
        //         });
        //         
        //     });
        //
        //     // Act
        //     var response = await client.PutAsync($"/api/Orders/Update/{orderId}",
        //         new StringContent(JsonSerializer.Serialize(orderUpdateDto), Encoding.UTF8, "application/json"));
        //
        //     // Assert
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }
    }
