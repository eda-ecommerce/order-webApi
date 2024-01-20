
using System.ComponentModel.DataAnnotations;
using Core.Models.DTOs.Order;

public class Order
{
    [Key]
    public Guid OrderId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public DateOnly OrderDate { get; set; }
    
    public OrderStatus OrderStatus { get; set; }
    
    public float TotalPrice { get; set; }
}
