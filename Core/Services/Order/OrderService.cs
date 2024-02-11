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

    public async Task<OrderDto?> UpdateOrder(Guid OrderId, OrderUpdateDto orderUpdateDto)
    {
        var order = await _orderRepository.GetOrder(OrderId);
        
        if (order == null)
        {
            _logger.LogInformation($"Order not found");
            return null;
        }

        if (orderUpdateDto.OrderStatus != OrderStatus.Cancelled &&
            orderUpdateDto.OrderStatus != OrderStatus.Completed && 
            orderUpdateDto.OrderStatus != OrderStatus.Paid &&
            orderUpdateDto.OrderStatus != OrderStatus.InProcess)
        {
            throw new Exception($"This order status not available: {orderUpdateDto.OrderStatus}");
        }
        
        order.OrderStatus = orderUpdateDto.OrderStatus;
    
        await _orderRepository.UpdateOrder(order);
    
        var updatedOrder = new OrderDto
        {
            OrderId = OrderId,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            OrderStatus = orderUpdateDto.OrderStatus,
            TotalPrice = order.TotalPrice,
            Items = order.Items
        };
        
        _logger.LogInformation($"Order was updated ${updatedOrder.OrderId}");
        return updatedOrder;
    }
}

