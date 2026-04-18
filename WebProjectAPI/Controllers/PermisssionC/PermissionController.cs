using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Attributes;
using WebProjectAPI.DTOs.PermissionDto;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Controllers.PermisssionC
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class PermissionController : ControllerBase
        {
            private readonly IPermissionService _service;

            public PermissionController(IPermissionService service)
            {
                _service = service;
            }

            [HttpGet("permissions")]
            public IActionResult GetAll()
            {
                return Ok(_service.GetAll());
            }

        [Authorize]
        [Permission("create_permission")]
        [HttpPost("permissions_add")]
            public IActionResult Add(PermissionCreateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(_service.Add(dto));
            }

        [Authorize]
        [Permission("edit_permission")]
        [HttpPut("permissions_edit")]
            public IActionResult Update(PermissionUpdateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(_service.Update(dto));
            }

        [Authorize]
        [Permission("delete_permission")]
        [HttpDelete("permissions_delete/{id}")]
            public IActionResult Delete(int id)
            {
                return Ok(_service.Delete(id));
            }

        }
}
