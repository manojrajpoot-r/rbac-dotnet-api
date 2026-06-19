using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.plans.DTOs;
using WebProjectAPI.Features.plans.Interfaces;
using WebProjectAPI.Features.plans.Services;

namespace WebProjectAPI.Features.plans.Controllers
{


  [Authorize]
    //[Permission("ManageTenants")]

    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        // ================= GET ALL =================

        [HttpPost("list")]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequest request)
        {
            var result = await _planService.GetAll(request);
            return Ok(result);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _planService.GetById(id);
            return Ok(result);
        }

        // ================= ADD =================
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateplanDto model)
        {
            var result = await _planService.Add(model);
            return Ok(result);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePlanDto model)
        {
            model.Id = id;

            var result = await _planService.Update(model);

            return Ok(result);
        }

        // ================= DELETE (SOFT DELETE) =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _planService.Delete(id);
            return Ok(result);
        }

        // ================= CHANGE STATUS =================
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _planService.ChangeStatus(id);
            return Ok(result);
        }

        // ================= DROPDOWN =================
        [HttpGet("dropdown")]
        public async Task<IActionResult> Dropdown()
        {
            var result = await _planService.Dropdown();
            return Ok(result);
        }
    }
}