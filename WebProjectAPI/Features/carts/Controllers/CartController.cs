using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.carts.DTOs;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart(
        AddToCartDto dto)
    {
        int userId = 1;

        var message = await _cartService
            .AddToCartAsync(userId, dto);

        if (message == "Product already added to cart")
        {
            return BadRequest(new
            {
                message = message
            });
        }

        return Ok(new
        {
            message = message
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        int userId = 1;

        var result = await _cartService
            .GetCartAsync(userId);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _cartService
            .RemoveCartAsync(id);

        return Ok(result);
    }

    [HttpPut("increase/{id}")]
    public async Task<IActionResult> Increase(int id)
    {
        var result = await _cartService
            .IncreaseQuantityAsync(id);

        return Ok(result);
    }

    [HttpPut("decrease/{id}")]
    public async Task<IActionResult> Decrease(int id)
    {
        var result = await _cartService
            .DecreaseQuantityAsync(id);

        return Ok(result);
    }
}