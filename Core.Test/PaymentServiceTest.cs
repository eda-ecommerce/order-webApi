﻿public class PaymentServiceTest
{
    private readonly OrderService _sut; //system unit test
    private readonly Mock<IOrderRepository> _paymentRepoMock = new Mock<IOrderRepository>();
    private readonly ILogger<OrderService> _logger;
    private readonly IConfiguration _configuration;


    public PaymentServiceTest()
    {
        _sut = new OrderService(_logger, _paymentRepoMock.Object, _configuration);
    }

    [Fact]
    public async Task GetPayments_ShouldReturnPayments_WhePaymentsExists()
    {
        // Arrage
        // payment 1
        var payment1Id = Guid.NewGuid();
        var payment1Firstname = "Hans";
        var payment1Lastname = "Dietmar";
        var payment1Username = "HansDietmar";
        var payment1 = new Order()
        {
            UserId = payment1Id,
            Firstname = payment1Firstname,
            Lastname = payment1Lastname,
            Username = payment1Username
        };

        // payment 2
        var payment2Id = Guid.NewGuid();
        var payment2Firstname = "Dieter";
        var payment2Lastname = "Mücke";
        var payment2Username = "DieterMücke";
        var payment2 = new Order()
        {
            UserId = payment2Id,
            Firstname = payment2Firstname,
            Lastname = payment2Lastname,
            Username = payment2Username
        };

        List<Order> paymentsList = new List<Order>() {};
        paymentsList.Add(payment1);
        paymentsList.Add(payment2);

        _paymentRepoMock.Setup(x => x.GetAllPayments())
            .ReturnsAsync(paymentsList);

        // Act
        var payments = await _sut.GetPayments();

        // Assert
        // Test payment 1
        payment1Id.Should().Be(payments[0].UserId.ToString());
        payment1Firstname.Should().Be(payments[0].Firstname);
        payment1Lastname.Should().Be(payments[0].Lastname);
        payment1Username.Should().Be(payments[0].Username);
        // Test payment 2
        payment2Id.Should().Be(payments[1].UserId.ToString());
        payment2Firstname.Should().Be(payments[1].Firstname);
        payment2Lastname.Should().Be(payments[1].Lastname);
        payment2Username.Should().Be(payments[1].Username);
    }

    [Fact]
    public async Task GetPayments_ShouldReturnNothing_WhenNoPaymentsExists()
    {
        // Arrage
        List<Order> paymentsList = new List<Order>() {};

        _paymentRepoMock.Setup(x => x.GetAllPayments())
            .ReturnsAsync(() => null);

        // Act
        var payments = await _sut.GetPayments();

        // Assert
        payments.Should().BeNull();
   
    }
}

