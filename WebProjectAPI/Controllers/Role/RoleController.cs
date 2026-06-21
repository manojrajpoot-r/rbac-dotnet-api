using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Attributes;
using WebProjectAPI.DTOs;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Controllers.Role
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [Authorize]
        [Permission("ROLE_VIEW")]

        [HttpPost("list")]

        public async Task<IActionResult> GetAll([FromBody] PaginationRequest request)
        {
            var result = await _service.GetAll(request);
            return Ok(result);
        }


        [Authorize]
        [Permission("ROLE_CREATE")]
        [HttpPost]
        public IActionResult Add(RoleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_service.Add(dto));
        }


        [Authorize]
        [Permission("ROLE_VIEW")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetById(id);
            return Ok(result);
        }


        [Authorize]
        [Permission("ROLE_EDIT")]
        [HttpPut("{id}")]
        public IActionResult Update(int id ,RoleUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            dto.Id = id;
            return Ok(_service.Update(dto));
        }

        [Authorize]
        [Permission("ROLE_DELETE")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }

        [Authorize]
        [Permission("ROLE_STATUS")]
        [HttpPatch("status/{id}")]
        public IActionResult Status(int id)
        {
            return Ok(_service.ToggleStatus(id));
        }
    }
}
