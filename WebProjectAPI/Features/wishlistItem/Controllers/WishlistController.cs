using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.wishlistItem.DTOs;
using WebProjectAPI.Features.wishlistItem.Models;
using WebProjectAPI.Services.Interfaces;
namespace WebProjectAPI.Features.wishlistItem.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public WishlistController(
            AppDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }


        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddWishlist(
        [FromBody] AddWishlistDto dto)
        {
            int userId =
                int.Parse(User.FindFirst("id")?.Value);

            var exists = await _context.Wishlists
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.ProductId == dto.ProductId &&
                    x.TenantId == _currentUser.TenantId);

            if (exists)
            {
                return BadRequest(new
                {
                    message = "Already Added"
                });
            }
            var product = await _context.Products
                    .FirstOrDefaultAsync(x =>
                    x.Id == dto.ProductId &&
                    x.TenantId == _currentUser.TenantId);

            if (product == null)
            {
                return BadRequest("Product not found");
            }

            var wishlist = new Wishlist
            {
                UserId = userId,
                ProductId = dto.ProductId,
                TenantId = _currentUser.TenantId.Value
            };


            _context.Wishlists.Add(wishlist);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Added To Wishlist"
            });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            int userId =
                int.Parse(User.FindFirst("id")?.Value);

            var data = await _context.Wishlists
                .Include(x => x.Product)
                .Where(x =>
                    x.UserId == userId &&
                    x.TenantId == _currentUser.TenantId)
                .Select(x => new
                {
                    x.Id,
                    x.ProductId,
                    x.Product.Name,
                    x.Product.Price,
                    x.Product.DiscountPrice,
                    x.Product.DiscountPercentage,
                    x.Product.Image,
                    x.Product.ShortDescription
                })
                .ToListAsync();

            return Ok(data);
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveWishlist(
            int id)
        {
            int userId =
                int.Parse(User.FindFirst("id")?.Value);

            var data = await _context.Wishlists
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == userId &&
                    x.TenantId == _currentUser.TenantId);

            if (data == null)
            {
                return NotFound();
            }

            _context.Wishlists.Remove(data);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Removed successfully"
            });
        }






    }
}
