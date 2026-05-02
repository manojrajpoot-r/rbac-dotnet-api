using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Categories.Interfaces;

namespace WebProjectAPI.Features.Categories.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var data = await _service.CreateAsync(dto);

            return Ok(new
            {
                message = "Category created successfully",
                data
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            return Ok(new
            {
                message = "Category updated successfully",
                data
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            return Ok(new
            {
                message = "Category deleted successfully"
            });
        }

        [HttpPatch("change-status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _service.ChangeStatusAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }

            return Ok(new
            {
                message = "Category status updated successfully"
            });
        }
    }
}