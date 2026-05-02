using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.sub_categories.DTOs;
using WebProjectAPI.Features.sub_categories.Interfaces;

namespace WebProjectAPI.Features.sub_categories.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase 
    {
        private readonly ISubCategoryService _service;

        public SubCategoryController(ISubCategoryService service)
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
        public async Task<IActionResult> Create([FromForm] CreateSubCategoryDto dto)
        {
            var data = await _service.CreateAsync(dto);

            return Ok(new
            {
                message = "SubCategory created successfully",
                data
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateSubCategoryDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "SubCategory not found"
                });
            }

            return Ok(new
            {
                message = "SubCategory updated successfully",
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
                    message = "SubCategory not found"
                });
            }

            return Ok(new
            {
                message = "SubCategory deleted successfully"
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
                    message = "SubCategory not found"
                });
            }

            return Ok(new
            {
                message = "SubCategory status updated successfully"
            });
        }
    }
}
