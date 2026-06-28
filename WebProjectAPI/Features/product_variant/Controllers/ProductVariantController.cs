using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using WebProjectAPI.Attributes;using WebProjectAPI.Features.Common.Paginations;using WebProjectAPI.Features.product_variant.DTOs;using WebProjectAPI.Features.product_variant.Interfaces;namespace WebProjectAPI.Features.product_variant.Controllers{	[Route("api/[controller]")]	[ApiController]	public class ProductVariantController : ControllerBase	{		private readonly IProductVariantService _service;

        public ProductVariantController(IProductVariantService service)		{			_service = service;		}
        [Authorize]
        [Permission("PRODUCT_VARIANT_VIEW")]
        [HttpPost("list")]		public async Task<IActionResult> List(PaginationRequest request)		{			var result = await _service.GetAll(request);			return Ok(result);		}		// GET BY ID		[HttpGet("{id}")]		public async Task<IActionResult> GetById(int id)		{			var result = await _service.GetById(id);			return Ok(result);		}

        [Authorize]
        [Permission("PRODUCT_VARIANT_CREATE")]
        [HttpPost]		public async Task<IActionResult> Add(ProductVariantCreateUpdateDto model)		{			var result = await _service.Add(model);			return Ok(result);		}

        [Authorize]
        [Permission("PRODUCT_VARIANT_EDIT")]
        [HttpPut("{id}")]		public async Task<IActionResult> Update(int id			,ProductVariantCreateUpdateDto model)		{			var result = await _service.Update(id,model);			return Ok(result);		}
        [Authorize]
        [Permission("PRODUCT_VARIANT_DELETE")]
        [HttpDelete("delete/{id}")]		public async Task<IActionResult> Delete(int id)		{			var result = await _service.Delete(id);			return Ok(result);		}

        [Authorize]
        [Permission("PRODUCT_VARIANT_STATUS")]
        [HttpPatch("status/{id}")]		public async Task<IActionResult> ChangeStatus(int id)		{			var result = await _service.ChangeStatus(id);			return Ok(result);		}		// frontend		[HttpGet("productBy/{id}")]		public async Task<IActionResult> GetByProductId(int id)		{			var result = await _service.GetByProductId(id);			return Ok(result);		}	}}