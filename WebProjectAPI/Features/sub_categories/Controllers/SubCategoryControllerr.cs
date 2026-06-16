using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.Common.Paginations;
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
        [Authorize]
        [HttpPost("list")]
        public async Task<IActionResult> GetAll(PaginationRequest request)
        {
            var data = await _service.GetAllAsync(request);
          
            return Ok(data);
        }
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

        // FRONTEND

        [HttpGet("frontend")]
        public async Task<IActionResult> GetAllSubCategories()
        {
            var data = await _service.GetAllSubCategoriesAsync();

            return Ok(data);
        }
    }
}
