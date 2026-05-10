using WebProjectAPI.Features.carts.DTOs;

public interface ICartService
{
    Task<bool> AddToCartAsync(
        int userId,
        AddToCartDto dto);

    Task<List<CartDto>> GetCartAsync(int userId);

    Task<bool> RemoveCartAsync(int id);

    Task<bool> IncreaseQuantityAsync(int id);

    Task<bool> DecreaseQuantityAsync(int id);
}