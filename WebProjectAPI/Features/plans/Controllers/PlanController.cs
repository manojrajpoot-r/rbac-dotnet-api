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
        private readonly IPlanService _tenantService;

        public PlanController(IPlanService tenantService)
        {
            _tenantService = tenantService;
        }

        // ================= GET ALL =================

        [HttpPost("list")]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequest request)
        {
            var result = await _tenantService.GetAll(request);
            return Ok(result);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tenantService.GetById(id);
            return Ok(result);
        }

        // ================= ADD =================
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePlanDto model)
        {
            var result = await _tenantService.Add(model);
            return Ok(result);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePlanDto model)
        {
            model.Id = id;

            var result = await _tenantService.Update(model);

            return Ok(result);
        }

        // ================= DELETE (SOFT DELETE) =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tenantService.Delete(id);
            return Ok(result);
        }

        // ================= CHANGE STATUS =================
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _tenantService.ChangeStatus(id);
            return Ok(result);
        }

        // ================= DROPDOWN =================
        [HttpGet("dropdown")]
        public async Task<IActionResult> Dropdown()
        {
            var result = await _tenantService.Dropdown();
            return Ok(result);
        }
    }
}