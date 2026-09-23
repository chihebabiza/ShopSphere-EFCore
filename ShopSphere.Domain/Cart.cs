namespace ShopSphere.Domain;

public class Cart : BaseEntity
{
    // Identifier for the user who owns the cart (could be GUID, username, or user id)
    public string UserId { get; set; } = string.Empty;

    // Navigation property for cart items
    public List<CartItem> Items { get; set; } = new();

    // Convenience property to compute total (not mapped if using EF conventions it will be ignored)
    public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
}
