using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.product_images.Interfaces;

namespace WebProjectAPI.Features.product_images.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductImageController : ControllerBase
{
    private readonly IProductImageService _service;

    public ProductImageController(IProductImageService service)
    {
        _service = service;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetByProductId(int productId)
    {
        return Ok(await _service.GetAllAsync(productId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromForm] ProductImageCreateDto dto)
    {
        return Ok(await _service.CreateAsync(dto.ProductId, dto.Images));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

    [HttpPut("set-primary/{id}")]
    public async Task<IActionResult> SetPrimary(int id)
    {
        return Ok(await _service.SetPrimaryAsync(id));
    }
}