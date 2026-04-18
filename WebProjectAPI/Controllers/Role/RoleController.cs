using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Attributes;
using WebProjectAPI.DTOs;
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

        [HttpGet("roles")]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [Authorize]
        [Permission("create_role")]
        [HttpPost("roles_add")]
        public IActionResult Add(RoleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_service.Add(dto));
        }
        [Authorize]
        [Permission("edit_role")]
        [HttpPut("roles_edit")]
        public IActionResult Update(RoleUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_service.Update(dto));
        }

        [Authorize]
        [Permission("delete_role")]
        [HttpDelete("roles_delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }

        [HttpPatch("roles_status/{id}")]
        public IActionResult Status(int id)
        {
            return Ok(_service.ToggleStatus(id));
        }
    }
}
