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

        [Authorize]
        [Permission("view_role")]
        [HttpGet]
        public IActionResult GetAll(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            var result = _service.GetAll(pageNumber,pageSize,search);
            return Ok(result);
        }

        [Authorize]
        [Permission("add_role")]
        [HttpPost]
        public IActionResult Add(RoleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_service.Add(dto));
        }


        [Authorize]
        [Permission("view_role")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetById(id);
            return Ok(result);
        }


        [Authorize]
        [Permission("edit_role")]
        [HttpPut("{id}")]
        public IActionResult Update(int id ,RoleUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            dto.Id = id;
            return Ok(_service.Update(dto));
        }

        [Authorize]
        [Permission("delete_role")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }

        [Authorize]
        [Permission("status_role")]
        [HttpPatch("{id}")]
        public IActionResult Status(int id)
        {
            return Ok(_service.ToggleStatus(id));
        }
    }
}
