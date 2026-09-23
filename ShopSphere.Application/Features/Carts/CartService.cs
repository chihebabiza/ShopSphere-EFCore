using ShopSphere.Application.Features.Carts.DTOs;
using ShopSphere.Application.Features.Products;
using ShopSphere.Domain;

namespace ShopSphere.Application.Features.Carts;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartResponseDto?> GetByUserIdAsync(string userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        return cart == null ? null : MapToDto(cart);
    }

    public async Task<CartResponseDto> CreateCartAsync(CreateCartRequest request)
    {
        var cart = new Cart
        {
            UserId = request.UserId,
            CreatedAtUtc = DateTime.UtcNow
        };

        var created = await _cartRepository.CreateCartAsync(cart);
        return MapToDto(created);
    }

    public async Task<CartResponseDto?> AddItemAsync(int cartId, AddCartItemRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null) throw new ArgumentException($"Product with ID {request.ProductId} not found.");

        var item = new CartItem
        {
            ProductId = product.Id,
            Quantity = request.Quantity,
            UnitPrice = product.Price
        };

        var updated = await _cartRepository.AddItemAsync(cartId, item);
        return updated == null ? null : MapToDto(updated);
    }

    public async Task<CartResponseDto?> UpdateItemQuantityAsync(int cartId, int itemId, UpdateCartItemRequest request)
    {
        var updated = await _cartRepository.UpdateItemQuantityAsync(cartId, itemId, request.Quantity);
        return updated == null ? null : MapToDto(updated);
    }

    public async Task<bool> RemoveItemAsync(int cartId, int itemId)
    {
        return await _cartRepository.RemoveItemAsync(cartId, itemId);
    }

    public async Task<bool> ClearCartAsync(int cartId)
    {
        return await _cartRepository.ClearCartAsync(cartId);
    }

    private static CartResponseDto MapToDto(Cart cart)
    {
        var items = cart.Items.Select(i => new CartItemResponseDto(
            i.Id,
            i.ProductId,
            i.Product?.Name,
            i.Quantity,
            i.UnitPrice
        )).ToList();

        return new CartResponseDto(
            cart.Id,
            cart.UserId,
            items,
            cart.Total
        );
    }
}
