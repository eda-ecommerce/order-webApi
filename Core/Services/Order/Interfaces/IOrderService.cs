using Core.Models.DTOs.Order;

public  interface IOrderService
{
    Task<List<OrderDto>?> GetOrders();
    Task<OrderDto?> GetOrder(Guid orderID);

    Task<OrderDto?> UpdateOrder(Guid orderId, OrderUpdateDto orderUpdateDto);
}

