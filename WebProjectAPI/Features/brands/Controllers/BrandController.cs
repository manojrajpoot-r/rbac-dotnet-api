using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.brands.Interfaces;


namespace WebProjectAPI.Features.Brands.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;

        public BrandsController(IBrandService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            var brands = await _service.GetAllAsync(pageNumber,pageSize, search);

            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _service.GetByIdAsync(id);

            if (brand == null)
                return NotFound();

            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBrandDto dto)
        {
            var brand = await _service.CreateAsync(dto);

            return Ok(new
            {
                message = "Brand created successfully",
                data = brand
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateBrandDto dto)
        {
            var brand = await _service.UpdateAsync(id, dto);

            if (brand == null)
                return NotFound();

            return Ok(new
            {
                message = "Brand updated successfully",
                data = brand
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.Data)
                return NotFound();

            return Ok(new
            {
                message = "Brand deleted successfully"
            });
        }

        [HttpPatch("change-status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _service.ChangeStatusAsync(id);

            if (!result.Data)
                return NotFound();

            return Ok(new
            {
                message = "Brand status changed successfully"
            });
        }
    }
}