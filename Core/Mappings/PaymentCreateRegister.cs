public class PaymentToPaymentDtoRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // usage:
        // var paymentDto = payment.Adapt<PaymentDto>();
        config.NewConfig<Order, OrderDto>().PreserveReference(true);
    }
}
