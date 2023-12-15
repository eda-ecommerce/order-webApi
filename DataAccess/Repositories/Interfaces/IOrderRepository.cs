public interface IOrderRepository
{
    Task<List<Order>> GetAllPayments();
    Task<Order> GetPayment(Guid paymentId);
    Task UpdatPayment(Order order);
}

