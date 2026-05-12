using WebProjectAPI.Features.carts.DTOs;
using WebProjectAPI.Features.carts.Interfaces;
using WebProjectAPI.Features.carts.Models;
namespace WebProjectAPI.Features.carts.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> AddToCartAsync(
       int userId,
       AddToCartDto dto)
        {
            var existing = await _repository
                .GetCartItemAsync(userId, dto.ProductId);

            if (existing != null)
            {
                return "Product already added to cart";
            }

            var cart = new Cart
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.Now
            };

            await _repository.AddAsync(cart);

            await _repository.SaveAsync();

            return "Added to cart successfully";
        }

        public async Task<List<CartDto>> GetCartAsync(int userId)
        {
            var carts = await _repository
                .GetUserCartAsync(userId);

            return carts.Select(x => new CartDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Image = x.Product.Image,
                Price = x.Product.Price,
                DiscountPrice = x.Product.DiscountPrice,
                Quantity = x.Quantity,
                Total =
                    (x.Product.DiscountPrice ?? x.Product.Price)
                    * x.Quantity

            }).ToList();
        }

        public async Task<bool> RemoveCartAsync(int id)
        {
            var cart = await _repository.GetByIdAsync(id);

            if (cart == null)
                return false;

            _repository.Remove(cart);

            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> IncreaseQuantityAsync(int id)
        {
            var cart = await _repository.GetByIdAsync(id);

            if (cart == null)
                return false;

            cart.Quantity++;

            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> DecreaseQuantityAsync(int id)
        {
            var cart = await _repository.GetByIdAsync(id);

            if (cart == null)
                return false;

            if (cart.Quantity > 1)
            {
                cart.Quantity--;
            }

            await _repository.SaveAsync();

            return true;
        }
    }

}