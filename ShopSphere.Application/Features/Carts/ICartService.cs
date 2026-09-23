using ShopSphere.Application.Features.Carts.DTOs;

namespace ShopSphere.Application.Features.Carts;

public interface ICartService
{
    Task<CartResponseDto?> GetByUserIdAsync(string userId);
    Task<CartResponseDto> CreateCartAsync(CreateCartRequest request);
    Task<CartResponseDto?> AddItemAsync(int cartId, AddCartItemRequest request);
    Task<CartResponseDto?> UpdateItemQuantityAsync(int cartId, int itemId, UpdateCartItemRequest request);
    Task<bool> RemoveItemAsync(int cartId, int itemId);
    Task<bool> ClearCartAsync(int cartId);
}
