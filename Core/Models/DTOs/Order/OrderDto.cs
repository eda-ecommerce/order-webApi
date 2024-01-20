using Core.Models.DTOs.Order;

public class OrderDto
{
    public Guid OrderId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public DateOnly OrderDate { get; set; }
    
    public OrderStatus OrderStatus { get; set; }
    
    public float TotalPrice { get; set; }
}