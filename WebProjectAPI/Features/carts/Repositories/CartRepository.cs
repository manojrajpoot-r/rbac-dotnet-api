
    using Microsoft.EntityFrameworkCore;
    using WebProjectAPI.Data;
    using WebProjectAPI.Features.carts.Interfaces;
    using WebProjectAPI.Features.carts.Models;
namespace WebProjectAPI.Features.carts.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartItemAsync(
            int userId,
            int productId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.ProductId == productId);
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task<List<Cart>> GetUserCartAsync(int userId)
        {
            return await _context.Carts
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Carts
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Remove(Cart cart)
        {
            _context.Carts.Remove(cart);
        }
    }
}
