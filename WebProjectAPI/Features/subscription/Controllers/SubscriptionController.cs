using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.subscription.DTOs;
using WebProjectAPI.Features.subscription.Interfaces;
using WebProjectAPI.Features.subscription.Services;

namespace WebProjectAPI.Features.subscription.Controllers
{




    [Authorize]
    //[Permission("ManageTenants")]

    [Route("api/[controller]")]
    [ApiController]

    public class SubscriptionController : ControllerBase
    {
        private readonly ITenantSubscriptionService _subscription;

        public SubscriptionController(ITenantSubscriptionService subscriptionService)
        {
            _subscription = subscriptionService;
        }

        // ================= GET ALL =================

        [HttpPost("list")]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequest request)
        {
            var result = await _subscription.GetAll(request);
            return Ok(result);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _subscription.GetById(id);
            return Ok(result);
        }

        // ================= ADD =================
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTenantSubscriptionDto model)
        {
            var result = await _subscription.Add(model);
            return Ok(result);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateTenantSubscriptionDto model)
        {
            model.Id = id;

            var result = await _subscription.Update(model);

            return Ok(result);
        }

        // ================= DELETE (SOFT DELETE) =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subscription.Delete(id);
            return Ok(result);
        }

        // ================= CHANGE STATUS =================
        [HttpPatch("status/{id}")]
    
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            return await _subscription.ChangeStatus(id);
        }


    }
}