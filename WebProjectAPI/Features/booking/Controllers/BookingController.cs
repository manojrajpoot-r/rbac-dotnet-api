using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.booking.DTOs;
using WebProjectAPI.Features.booking.Interfaces;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(
            IBookingService service)
        {
            _service = service;
        }

        // LIST
        [HttpPost("list")]
        public async Task<IActionResult> List(
            PaginationRequest request)
        {
            var result = await _service.GetAll(request);

            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var result = await _service.GetById(id);

            return Ok(result);
        }

        // ADD
        [HttpPost("add")]
        public async Task<IActionResult> Add(
            CreateBookingDto model)
        {
            var result = await _service.Add(model);

            return Ok(result);
        }

        // UPDATE
        [HttpPut("update")]
        public async Task<IActionResult> Update(
            UpdateBookingDto model)
        {
            var result = await _service.Update(model);

            return Ok(result);
        }

        // DELETE
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var result = await _service.Delete(id);

            return Ok(result);
        }

        // STATUS
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> ChangeStatus(
            int id)
        {
            var result = await _service.ChangeStatus(id);

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _service.GetByUser(userId);

            return Ok(result);
        }
    }
}