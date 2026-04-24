using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

            [HttpGet]
            public IActionResult GetAll()
            {
                return Ok(_service.GetAll());
            }


        //[Authorize]
        //[Permission("create_user")]
        [HttpPost]
        public IActionResult Add(UserCreateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(_service.Add(dto));
            }


        //[Authorize]
        //[Permission("edit_user")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetById(id);
            return Ok(result);
        }




        //[Authorize]
        //[Permission("edit_user")]
        [HttpPut("{id}")]
        public IActionResult Update(int id,UserUpdateDto dto)
        {
            dto.Id = id;
            return Ok(_service.Update(dto));
        }

        //[Authorize]
        //[Permission("delete_user")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
                return Ok(_service.Delete(id));
        }

        [HttpPatch("{id}")]
        public IActionResult Status(int id)
        {
            return Ok(_service.ToggleStatus(id));
        }
        }
    }

