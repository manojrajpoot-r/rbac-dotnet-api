using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.carts.DTOs;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetUserId()
    {
        return Convert.ToInt32(
            User.FindFirst("id")?.Value
        );
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart(
        AddToCartDto dto)
    {
        int userId = GetUserId();

        var message = await _cartService
            .AddToCartAsync(userId, dto);

        return Ok(new
        {
            message = message
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        int userId = GetUserId();

        var result = await _cartService
            .GetCartAsync(userId);

        return Ok(result);
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        int userId = GetUserId();

        var count = await _cartService
            .GetCartCountAsync(userId);

        return Ok(new
        {
            count
        });
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        int userId = GetUserId();

        var result = await _cartService
            .ClearCartAsync(userId);

        return Ok(new
        {
            success = result
        });
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