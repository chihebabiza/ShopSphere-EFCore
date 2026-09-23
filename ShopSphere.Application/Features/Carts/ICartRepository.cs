using ShopSphere.Application.Common.Interfaces;
using ShopSphere.Domain;

namespace ShopSphere.Application.Features.Carts;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<Cart?> GetByUserIdAsync(string userId);
    Task<Cart?> GetWithItemsAsync(int cartId);
    Task<Cart> CreateCartAsync(Cart cart);
    Task<Cart?> AddItemAsync(int cartId, CartItem item);
    Task<Cart?> UpdateItemQuantityAsync(int cartId, int itemId, int quantity);
    Task<bool> RemoveItemAsync(int cartId, int itemId);
    Task<bool> ClearCartAsync(int cartId);
}
