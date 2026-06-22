// ================================
// CartRepository
// ================================

using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.carts.Interfaces;
using WebProjectAPI.Features.carts.Models;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Features.carts.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public CartRepository(
            AppDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Cart?> GetCartItemAsync(
         int userId,
         int productId)
        {
            return await _context.Carts
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.ProductId == productId &&
                    x.TenantId == _currentUser.TenantId);
        }
        public async Task<List<Cart>> GetUserCartAsync(
            int userId)
        {
            return await _context.Carts
                .Include(x => x.Product)
                .Where(x =>
                    x.UserId == userId &&
                    x.TenantId == _currentUser.TenantId)
                .ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Carts
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);
        }

        public async Task<int> GetCartCountAsync(
           int userId)
        {
            return await _context.Carts
                .Where(x =>
                    x.UserId == userId &&
                    x.TenantId == _currentUser.TenantId)
                .CountAsync();
        }

        public async Task ClearCartAsync(int userId)
        {
            var carts = await _context.Carts
                .Where(x =>
                    x.UserId == userId &&
                    x.TenantId == _currentUser.TenantId)
                .ToListAsync();

            _context.Carts.RemoveRange(carts);

            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Cart cart)
        {
            cart.TenantId = _currentUser.TenantId.Value;

            await _context.Carts.AddAsync(cart);
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