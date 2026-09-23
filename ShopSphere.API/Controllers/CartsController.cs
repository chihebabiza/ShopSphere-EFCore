using Microsoft.AspNetCore.Mvc;
using ShopSphere.Application.Features.Carts;
using ShopSphere.Application.Features.Carts.DTOs;

namespace ShopSphere.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<CartResponseDto>> GetByUser(string userId)
    {
        var cart = await _cartService.GetByUserIdAsync(userId);
        if (cart == null) return NotFound(new { message = "Cart not found." });
        return Ok(cart);
    }

    [HttpPost]
    public async Task<ActionResult<CartResponseDto>> Create([FromBody] CreateCartRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _cartService.CreateCartAsync(request);
        return CreatedAtAction(nameof(GetByUser), new { userId = created.UserId }, created);
    }

    [HttpPost("{cartId:int}/items")]
    public async Task<ActionResult<CartResponseDto>> AddItem(int cartId, [FromBody] AddCartItemRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await _cartService.AddItemAsync(cartId, request);
            if (updated == null) return NotFound(new { message = "Cart not found." });
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{cartId:int}/items/{itemId:int}")]
    public async Task<ActionResult<CartResponseDto>> UpdateItem(int cartId, int itemId, [FromBody] UpdateCartItemRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _cartService.UpdateItemQuantityAsync(cartId, itemId, request);
        if (updated == null) return NotFound(new { message = "Cart or item not found." });
        return Ok(updated);
    }

    [HttpDelete("{cartId:int}/items/{itemId:int}")]
    public async Task<IActionResult> RemoveItem(int cartId, int itemId)
    {
        var removed = await _cartService.RemoveItemAsync(cartId, itemId);
        if (!removed) return NotFound(new { message = "Cart or item not found." });
        return NoContent();
    }

    [HttpDelete("{cartId:int}/items")]
    public async Task<IActionResult> ClearCart(int cartId)
    {
        var cleared = await _cartService.ClearCartAsync(cartId);
        if (!cleared) return NotFound(new { message = "Cart not found." });
        return NoContent();
    }
}
