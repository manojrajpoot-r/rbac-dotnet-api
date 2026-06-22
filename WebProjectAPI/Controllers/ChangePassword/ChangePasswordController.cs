using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Features.colors.Interfaces;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Controllers.changePassword
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly IUserService _service;

        public ChangePasswordController(IUserService service)
        {
            _service = service;
        }

   
        [HttpPost("User/ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _service.ChangePasswordAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
