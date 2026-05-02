using WebProjectAPI.Features.products.Models;

namespace WebProjectAPI.Features.product_images.Models
{
   
        public class ProductImage
        {
            public int Id { get; set; }

            public int ProductId { get; set; }

            public string ImageUrl { get; set; } = string.Empty;

            public bool IsMain { get; set; } = false;

            public Product Product { get; set; }
        
    }
}
