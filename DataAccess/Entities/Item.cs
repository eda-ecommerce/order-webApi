namespace Core.Models.DTOs.Order;

public class Item
{
    public Guid ItemId { get; set; }
    
    public int Quantity { get; set; }

    public Guid OfferingId { get; set; }
    
    public Guid OrderId { get; set; }

    public global::Order Order { get; set; }
    
    public float TotalPrice { get; set; }
    
}