using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Attributes;
using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Categories.Interfaces;
using WebProjectAPI.Features.Common.Paginations;

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


        [Authorize]
        [Permission("CATEGORY_VIEW")]
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
        [Permission("CATEGORY_CREATE")]
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

        [Authorize]
        [Permission("CATEGORY_UPDATE")]
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

        [Authorize]
        [Permission("CATEGORY_DELETE")]
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

        [Authorize]
        [Permission("CATEGORY_STATUS")]
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


        // FRONTEND

        [HttpGet("frontend")]
        public async Task<IActionResult> GetAllCategories()
        {
            var data = await _service.GetCategoriesAsync();

            return Ok(data);
        }
    }
}