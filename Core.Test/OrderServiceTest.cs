public class OrderServiceTest
{
    private readonly OrderService _sut; //system unit test
    private readonly Mock<IOrderRepository> _paymentRepoMock = new Mock<IOrderRepository>();
    private readonly ILogger<OrderService> _logger;
    private readonly IConfiguration _configuration;


    // public OrderServiceTest()
    // {
    //     _sut = new OrderService(_logger, _orderRepoMock.Object, _configuration);
    // }
    //
    // [Fact]
    // public async Task GetOrders_ShouldReturnOrders_WheOrdersExists()
    // {
    //     // Arrage
    //     // order 1
    //     var order1Id = Guid.NewGuid();
    //     var order1Firstname = "Hans";
    //     var order1Lastname = "Dietmar";
    //     var order1Username = "HansDietmar";
    //     var order1 = new Order()
    //     {
    //         UserId = order1Id,
    //         Firstname = order1Firstname,
    //         Lastname = order1Lastname,
    //         Username = order1Username
    //     };
    //
    //     // order 2
    //     var order2Id = Guid.NewGuid();
    //     var order2Firstname = "Dieter";
    //     var order2Lastname = "Mücke";
    //     var order2Username = "DieterMücke";
    //     var order2 = new Order()
    //     {
    //         UserId = order2Id,
    //         Firstname = order2Firstname,
    //         Lastname = order2Lastname,
    //         Username = order2Username
    //     };
    //
    //     List<Order> ordersList = new List<Order>() {};
    //     ordersList.Add(order1);
    //     ordersList.Add(order2);
    //
    //     _orderRepoMock.Setup(x => x.GetAllOrders())
    //         .ReturnsAsync(ordersList);
    //
    //     // Act
    //     var orders = await _sut.GetOrders();
    //
    //     // Assert
    //     // Test order 1
    //     order1Id.Should().Be(orders[0].UserId.ToString());
    //     order1Firstname.Should().Be(orders[0].Firstname);
    //     order1Lastname.Should().Be(orders[0].Lastname);
    //     order1Username.Should().Be(orders[0].Username);
    //     // Test order 2
    //     order2Id.Should().Be(orders[1].UserId.ToString());
    //     order2Firstname.Should().Be(orders[1].Firstname);
    //     order2Lastname.Should().Be(orders[1].Lastname);
    //     order2Username.Should().Be(orders[1].Username);
    // }
    //
    // [Fact]
    // public async Task GetOrders_ShouldReturnNothing_WhenNoOrdersExists()
    // {
    //     // Arrage
    //     List<Order> ordersList = new List<Order>() {};
    //
    //     _orderRepoMock.Setup(x => x.GetAllOrders())
    //         .ReturnsAsync(() => null);
    //
    //     // Act
    //     var orders = await _sut.GetOrders();
    //
    //     // Assert
    //     orders.Should().BeNull();
    //
    // }
}

