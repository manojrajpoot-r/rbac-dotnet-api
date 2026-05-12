// ================================
// ICartService
// ================================

using WebProjectAPI.Features.carts.DTOs;

public interface ICartService
{
    Task<string> AddToCartAsync(
        int userId,
        AddToCartDto dto);

    Task<List<CartDto>> GetCartAsync(int userId);

    Task<bool> RemoveCartAsync(int id);

    Task<bool> IncreaseQuantityAsync(int id);

    Task<bool> DecreaseQuantityAsync(int id);

    Task<int> GetCartCountAsync(int userId);

    Task<bool> ClearCartAsync(int userId);
}