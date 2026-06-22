using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.colors.DTOs;
using WebProjectAPI.Features.colors.Interfaces;
using WebProjectAPI.Features.colors.Services;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.colors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _service;

        public ColorController(IColorService service)
        {
            _service = service;
        }

        // LIST
        [HttpPost("list")]
        public async Task<IActionResult> List(PaginationRequest request)
        {
            var result = await _service.GetAll(request);

            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);

            return Ok(result);
        }

        // ADD
        [HttpPost("add")]
        public async Task<IActionResult> Add(ColorDto model)
        {
            var result = await _service.Add(model);

            return Ok(result);
        }

   

        [HttpPut("update")]
        public async Task<IActionResult> Update(ColorDto model)
        {
            var result = await _service.Update(model);

            return Ok(result);
        }

        // DELETE
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);

            return Ok(result);
        }

        // STATUS CHANGE
        [HttpPatch("status/{id}")]
       
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _service.ChangeStatusAsync(id);
            return Ok(result);
        }

        // DROPDOWN
        [HttpGet("frontend")]
        public async Task<IActionResult> Dropdown()
        {
            var result = await _service.Dropdown();

            return Ok(result);
        }
    }
}
