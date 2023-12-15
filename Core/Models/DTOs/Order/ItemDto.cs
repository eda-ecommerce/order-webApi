using System.ComponentModel.DataAnnotations;

public class ItemDto
{
    [Key]
    public Guid ItemId { get; set; }
    
    public int Quantity { get; set; }

    public OfferingDto Offering { get; set; }
}