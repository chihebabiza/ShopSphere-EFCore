using Microsoft.AspNetCore.Mvc;
using ShopSphere.Application.Features.Products;
using ShopSphere.Application.Features.Products.DTOs;

namespace ShopSphere.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponseDto>>> GetAll(
        [FromQuery] int? categoryId = null,
        [FromQuery] bool? onlyActive = null,
        [FromQuery] bool? onlyFeatured = null)
    {
        var products = await _productService.GetAllAsync(categoryId, onlyActive, onlyFeatured);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID {id} was not found." });
        }

        return Ok(product);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ProductResponseDto>> GetBySlug(string slug)
    {
        var product = await _productService.GetBySlugAsync(slug);
        if (product == null)
        {
            return NotFound(new { message = $"Product with slug '{slug}' was not found." });
        }

        return Ok(product);
    }

    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<ProductResponseDto>> GetBySku(string sku)
    {
        var product = await _productService.GetBySkuAsync(sku);
        if (product == null)
        {
            return NotFound(new { message = $"Product with SKU '{sku}' was not found." });
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _productService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> Update(int id, [FromBody] UpdateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updated = await _productService.UpdateAsync(id, request);
            if (updated == null)
            {
                return NotFound(new { message = $"Product with ID {id} was not found." });
            }

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Product with ID {id} was not found." });
        }

        return NoContent();
    }
}
