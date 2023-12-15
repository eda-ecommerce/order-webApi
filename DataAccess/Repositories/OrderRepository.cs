public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllOrders()
    {
        var orders = await _context.Orders
            .ToListAsync();

        return orders;
    }
    public async Task<Order> GetOrder(Guid orderId)
    {
        var order = await _context.Orders
            .Where(p => p.UserId == orderId)
            .FirstOrDefaultAsync();
        return order;
    }

    public async Task UpdatOrder(Order order)
    {
        var orderToUpdate = await _context.Orders
            .Where(p => p.UserId == order.UserId)
            .FirstOrDefaultAsync();

        orderToUpdate = order;

        _context.Update(orderToUpdate);
        _context.SaveChanges();
    }

}

