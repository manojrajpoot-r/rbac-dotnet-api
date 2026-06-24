using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.sizes.Interfaces;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.sizes.DTOs;
using WebProjectAPI.Attributes;
using Microsoft.AspNetCore.Authorization;
namespace WebProjectAPI.Features.sizes.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class SizeController : ControllerBase
        {
            private readonly ISizeService _service;

            public SizeController(ISizeService service)
            {
                _service = service;
            }

             [Authorize]
             [Permission("SIZE_VIEW")]
            [HttpPost("list")]
            public async Task<IActionResult> List(PaginationRequest request)
            {
                var result = await _service.GetAll(request);

                return Ok(result);
            }


        [Authorize]
    
        [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var result = await _service.GetById(id);

                return Ok(result);
            }


                [Authorize]
                [Permission("SIZE_CREATE")]
                [HttpPost("add")]
            public async Task<IActionResult> Add(SizeDto model)
            {
                var result = await _service.Add(model);

                return Ok(result);
            }


            [Authorize]
            [Permission("SIZE_UPDATE")]
            [HttpPut("update")]
            public async Task<IActionResult> Update(SizeDto model)
            {
                var result = await _service.Update(model);

                return Ok(result);
            }


            [Authorize]
            [Permission("SIZE_DELETE")]
            [HttpDelete("delete/{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var result = await _service.Delete(id);

                return Ok(result);
            }


            [Authorize]
            [Permission("SIZE_STATUS")]
            [HttpPatch("status/{id}")]
            public async Task<IActionResult> ChangeStatus(int id)
            {
                var result = await _service.ChangeStatus(id);

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
