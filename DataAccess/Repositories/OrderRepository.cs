public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllOrders()
    {
        var orders = await _context.Orders.ToListAsync();

        return orders;
    }
    public async Task<Order> GetOrder(Guid orderId)
    {
        var order = await _context.Orders
            .Where(p => p.OrderId == orderId)
            .FirstOrDefaultAsync();
        return order;
    }

    public async Task UpdateOrder(Order order)
    {
        var orderToUpdate = await _context.Orders
            .Where(p => p.OrderId == order.OrderId)
            .FirstOrDefaultAsync();

        orderToUpdate = order;

        _context.Update(orderToUpdate);
        _context.SaveChanges();
    }

}

