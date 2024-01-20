public interface IOrderRepository
{
    Task<List<Order>> GetAllOrders();
    Task<Order> GetOrder(Guid paymentId);
    Task UpdateOrder(Order order);
}

