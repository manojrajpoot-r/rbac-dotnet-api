using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Attributes;
using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Repositories.Implementations;
using WebProjectAPI.Repositories.Interfaces;
using WebProjectAPI.Services.Implementations;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Controllers.UserC
{

        [ApiController]
        [Route("api/[controller]")]
        public class UserController : ControllerBase
        {
            private readonly IUserService _service;

            public UserController(IUserService service)
            {
                _service = service;
            }

            [HttpGet("users")]
            public IActionResult GetAll()
            {
                return Ok(_service.GetAll());
            }


        [Authorize]
        [Permission("create_user")]
        [HttpPost("users_add")]
            public IActionResult Add(UserCreateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(_service.Add(dto));
            }

        [Authorize]
        [Permission("edit_user")]
        [HttpPut("users_edit")]
            public IActionResult Update(UserUpdateDto dto)
            {
                return Ok(_service.Update(dto));
            }
        [Authorize]
        [Permission("delete_user")]
        [HttpDelete("users_delete/{id}")]
            public IActionResult Delete(int id)
            {
                return Ok(_service.Delete(id));
            }

            [HttpPatch("users_status/{id}")]
            public IActionResult Status(int id)
            {
                return Ok(_service.ToggleStatus(id));
            }
        }
    }

