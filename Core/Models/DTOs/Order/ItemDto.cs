using System.ComponentModel.DataAnnotations;

public class ItemDto
{
    [Key]
    public Guid ItemId { get; set; }
    
    public int Quantity { get; set; }
    
    public Guid OrderId { get; set; }

    public Order Order { get; set; }

    public Guid OfferingId { get; set; }
    
    public float TotalPrice { get; set; }
}