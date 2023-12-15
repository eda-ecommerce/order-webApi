using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _OrderRepository;
    private readonly IConfiguration _configuration;

    public OrderService(ILogger<OrderService> logger, IOrderRepository orderRepository, IConfiguration configuration)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _configuration = configuration;
    }



    public async Task<List<OrderDto>?> GetOrders()
    {
        var orders = await _orderRepository.GetAllOrders();

        if (orders == null)
        {
            return null;
        }

        var orderDto = orders.Adapt<List<OrderDto>>();

        return orderDto;
    }

    public async Task<OrderDto?> GetOrder(Guid orderId)
    {
        var order = await _orderRepository.GetOrder(orderId);

        if (order == null)
        {
            return null;
        }

        // Map to Dto
        var orderDto = order.Adapt<OrderDto>();

        return orderDto;
    }

    public async Task UpdateOrder(OfferingDto offeringDto)
    {
        // get topic from appsettings.json
        var kafka_topic = _configuration.GetSection("Kafka").GetSection("Topic1").Value;
        var kafka_broker = _configuration.GetSection("Kafka").GetSection("Broker").Value;
        _logger.LogInformation($" Topic: {kafka_topic}");

        var order = await _orderRepository.GetOrder((Guid)offeringDto.UserId);
        

        if (order == null)
        {
            throw new Exception($"Order not found: {offeringDto.UserId}");
        }

        order.Firstname = offeringDto.Firstname;
        order.Lastname = offeringDto.Lastname;
        order.Username = offeringDto.Username;

        await _orderRepository.UpdatOrder(order);

        // Produce messages
        ProducerConfig configProducer = new ProducerConfig
        {
            BootstrapServers = kafka_broker,
            ClientId = Dns.GetHostName()
        };

        var demoOrder = new OrderDto
        {
            Firstname = offeringDto.Firstname,
            Lastname = offeringDto.Lastname,
            Username = offeringDto.Username
        };

        using var producer = new ProducerBuilder<Null, string>(configProducer).Build();
        
        var result = await producer.ProduceAsync(kafka_topic, new Message<Null, string>
        {
            Value = JsonSerializer.Serialize<OrderDto>(demoOrder)
        });
        Console.WriteLine(JsonSerializer.Serialize<OrderDto>(demoOrder));

        
    }
}

