using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.wishlistItem.DTOs;
using WebProjectAPI.Features.wishlistItem.Models;
namespace WebProjectAPI.Features.wishlistItem.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WishlistController(AppDbContext context)
        {
            _context = context;
        }

       

       
      


        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddWishlist([FromBody] AddWishlistDto dto)
        {
            int userId =
                int.Parse(
                    User.FindFirst("id")?.Value
                );

            var exists = await _context.Wishlists
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.ProductId == dto.ProductId);

            if (exists)
            {
                return BadRequest(new
                {
                    message = "Already Added"
                });
            }

            var wishlist = new Wishlist
            {
                UserId = userId,
                ProductId = dto.ProductId
            };

            _context.Wishlists.Add(wishlist);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Added To Wishlist"
            });
        }



        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            var data = await _context.Wishlists

                .Include(x => x.Product)

                .Where(x => x.UserId == userId)

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


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveWishlist(int id)
        {
            var data = await _context.Wishlists.FindAsync(id);

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
