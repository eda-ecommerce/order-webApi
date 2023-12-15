using System.ComponentModel.DataAnnotations;

public class OfferingDto
{
    [Key]
    public Guid OfferingId { get; set; }

    public Guid ProductId { get; set; }
    
    public int Quantity { get; set; }

    public float Price { get; set; }

    public bool Status { get; set; }
}

