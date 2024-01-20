namespace Core.Models.DTOs.Order;

public class OrderUpdateDto
{
    public OrderStatus OrderStatus { get; set; }
    
    public float TotalPrice { get; set; }
}