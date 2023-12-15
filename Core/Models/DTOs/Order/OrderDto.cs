using System.ComponentModel.DataAnnotations;

public class OrderDto
{
    [Key]
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; }
    public float TotalPrice { get; set; }
    public List<ItemDto> Items { get; set; }
}