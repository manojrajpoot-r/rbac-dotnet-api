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

        [Authorize]
        [Permission("view_permission")]
        [HttpGet]
        public IActionResult GetAll(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            var result = _service.GetAll(pageNumber, pageSize, search);
            return Ok(result);
        }


        [Authorize]
        [Permission("add_permission")]
        [HttpPost]
            public IActionResult Add(PermissionCreateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(_service.Add(dto));
            }



        [Authorize]
        [Permission("view_permission")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetById(id);
            return Ok(result);
        }

        [Authorize]
        [Permission("edit_permission")]
        [HttpPut]
            public IActionResult Update(PermissionUpdateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(_service.Update(dto));
            }

        [Authorize]
        [Permission("delete_permission")]
        [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                return Ok(_service.Delete(id));
            }

        }
}
