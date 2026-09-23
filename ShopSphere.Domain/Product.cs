namespace ShopSphere.Domain;

public class Product : BaseEntity
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }

    // Pricing
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public decimal? CostPrice { get; set; }

    // Inventory & Logistics
    public int StockQuantity { get; set; } = 0;
    public int LowStockThreshold { get; set; } = 5;
    public decimal? WeightKg { get; set; }

    // Status
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;

    // Foreign Key & Navigation Property
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}