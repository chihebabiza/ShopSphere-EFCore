namespace ShopSphere.Domain;

public class CartItem : BaseEntity
{
    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 1;

    // Capture unit price at time item was added to cart to avoid price drift
    public decimal UnitPrice { get; set; }
}
