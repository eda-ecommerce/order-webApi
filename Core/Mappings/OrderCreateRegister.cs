public class OrderToOrderDtoRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // usage:
        // var orderDto = order.Adapt<OrderDto>();
        config.NewConfig<Order, OrderDto>().PreserveReference(true);
    }
}
