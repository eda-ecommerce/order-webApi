using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using Core.Models.DTOs.Order;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
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

        if (orders.Count == 0)
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

    public async Task UpdateOrder(Guid OrderId, OrderUpdateDto orderUpdateDto)
    {
        // get topic from appsettings.json
        var kafka_topic = _configuration.GetSection("Kafka").GetSection("Topic1").Value;
        var kafka_broker = _configuration.GetSection("Kafka").GetSection("Broker").Value;
        _logger.LogInformation($" Topic: {kafka_topic}");
    
        var order = await _orderRepository.GetOrder(OrderId);
        
    
        if (order == null)
        {
            throw new Exception($"Order not found: {OrderId}");
        }

        if (orderUpdateDto.OrderStatus != OrderStatus.Cancelled &&
            orderUpdateDto.OrderStatus != OrderStatus.Completed && 
            orderUpdateDto.OrderStatus != OrderStatus.Paid &&
            orderUpdateDto.OrderStatus != OrderStatus.InProgress)
        {
            throw new Exception($"This order status not available: {orderUpdateDto.OrderStatus}");
        }
        
        order.OrderStatus = orderUpdateDto.OrderStatus;
    
        await _orderRepository.UpdateOrder(order);
    
        // Produce messages
        ProducerConfig configProducer = new ProducerConfig
        {
            BootstrapServers = kafka_broker,
            ClientId = Dns.GetHostName()
        };

      
        var orderHeader = new Headers();
        orderHeader.Add("source", Encoding.UTF8.GetBytes("order"));
        orderHeader.Add("timestamp", Encoding.UTF8.GetBytes(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()));
        orderHeader.Add("operation", Encoding.UTF8.GetBytes("updated"));
        
    
        var demoOrder = new OrderDto
        {
            OrderStatus = orderUpdateDto.OrderStatus,
        };
    
        using var producer = new ProducerBuilder<Null, string>(configProducer).Build();
        
        var result = await producer.ProduceAsync(kafka_topic, new Message<Null, string>
        {
            Value = JsonSerializer.Serialize<OrderDto>(demoOrder),
            Headers = orderHeader
        });

    }
}

