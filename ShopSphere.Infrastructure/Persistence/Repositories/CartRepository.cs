using Microsoft.EntityFrameworkCore;
using ShopSphere.Application.Features.Carts;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence.Repositories;

public class CartRepository : EfRepository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Cart?> GetByUserIdAsync(string userId)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart?> GetWithItemsAsync(int cartId)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }

    public async Task<Cart> CreateCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart?> AddItemAsync(int cartId, CartItem item)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null) return null;

        var existing = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existing != null)
        {
            existing.Quantity += item.Quantity;
            // update unit price if different
            existing.UnitPrice = item.UnitPrice;
        }
        else
        {
            cart.Items.Add(item);
        }

        await _context.SaveChangesAsync();
        return await GetWithItemsAsync(cartId);
    }

    public async Task<Cart?> UpdateItemQuantityAsync(int cartId, int itemId, int quantity)
    {
        var item = await _context.CartItems
            .Include(i => i.Cart)
            .FirstOrDefaultAsync(i => i.Id == itemId && i.CartId == cartId);

        if (item == null) return null;

        if (quantity <= 0)
        {
            _context.CartItems.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }

        await _context.SaveChangesAsync();
        return await GetWithItemsAsync(cartId);
    }

    public async Task<bool> RemoveItemAsync(int cartId, int itemId)
    {
        var item = await _context.CartItems
            .FirstOrDefaultAsync(i => i.Id == itemId && i.CartId == cartId);

        if (item == null) return false;

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCartAsync(int cartId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null) return false;

        _context.CartItems.RemoveRange(cart.Items);
        await _context.SaveChangesAsync();
        return true;
    }
}
