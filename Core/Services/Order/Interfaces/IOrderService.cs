public  interface IOrderService
{
    Task<List<OrderDto>?> GetOrders();
    Task<OrderDto?> GetOrder(Guid orderID);
    Task UpdateOrder(OfferingDto offeringDto);
}

