﻿using System.Net;
using System.Text;
using Confluent.Kafka;
using Core.Models.DTOs.Order;

[Route("api/Orders")]
[ApiController]
public class OrdersController : ControllerBase
{

    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderService _orderService;
    private readonly IConfiguration _configuration;

    public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        _logger.LogInformation($"Get orders request");

        List<OrderDto> orders = null;
        orders = await _orderService.GetOrders();
        
        if (orders == null)
        {
            return NotFound($"Orders not found");
        }

        return Ok(orders);
    }
    
    [HttpGet("{id}/Order")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        _logger.LogInformation($"Get orders request");
    
        OrderDto order = null;
        order = await _orderService.GetOrder(id);
        
        if (order == null)
        {
            return NotFound($"Order not found: {id}");
        }
    
        return Ok(order);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderUpdateDto OrderUpdateDto)
    {
        // get topic from appsettings.json
        var kafka_topic = _configuration.GetSection("Kafka").GetSection("Topic1").Value;
        var kafka_broker = _configuration.GetSection("Kafka").GetSection("Broker").Value;
        _logger.LogInformation($" Topic: {kafka_topic}");
        
        // Find order
        var order = await _orderService.GetOrder(id);
    
        if (order == null)
        {
            return NotFound($"Order not found: {id}");
        }
    
        try
        {
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

            var updatedOrder = await _orderService.UpdateOrder(id, OrderUpdateDto);
            
            using var producer = new ProducerBuilder<Null, string>(configProducer).Build();
        
            var result = await producer.ProduceAsync(kafka_topic, new Message<Null, string>
            {
                Value = JsonSerializer.Serialize<OrderDto>(updatedOrder),
                Headers = orderHeader
            });
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    
        return Ok($"Order Updated Successfully: {id}");
    }
}

