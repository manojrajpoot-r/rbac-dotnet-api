using WebProjectAPI.Features.carts.Models;

namespace WebProjectAPI.Features.carts.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartItemAsync(
            int userId,
            int productId);

        Task AddAsync(Cart cart);

        Task<List<Cart>> GetUserCartAsync(int userId);

        Task<Cart?> GetByIdAsync(int id);

        Task SaveAsync();

        void Remove(Cart cart);
    }
}
