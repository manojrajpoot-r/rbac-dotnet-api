using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.product_images.Interfaces;

namespace WebProjectAPI.Features.product_images.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _service;

        public ProductImageController(IProductImageService service)
        {
            _service = service;
        }

    
        [HttpGet]
        public async Task<IActionResult> GetAll(int productId)
        {
            var result = await _service.GetAllAsync(productId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromForm] ProductImageCreateDto dto)
        {
            var result = await _service.CreateAsync(
                dto.ProductId,
                dto.Images);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] ProductImageUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}