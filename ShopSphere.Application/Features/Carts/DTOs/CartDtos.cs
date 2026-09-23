using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Application.Features.Carts.DTOs;

public record CartItemResponseDto(
    int Id,
    int ProductId,
    string? ProductName,
    int Quantity,
    decimal UnitPrice
);

public record CartResponseDto(
    int Id,
    string UserId,
    IReadOnlyList<CartItemResponseDto> Items,
    decimal Total
);

public record CreateCartRequest(
    [Required] string UserId
);

public record AddCartItemRequest(
    [Required] int ProductId,
    [Range(1, int.MaxValue)] int Quantity
);

public record UpdateCartItemRequest(
    [Range(0, int.MaxValue)] int Quantity
);
