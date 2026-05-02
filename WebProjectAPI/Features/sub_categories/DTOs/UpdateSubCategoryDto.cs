namespace WebProjectAPI.Features.sub_categories.DTOs
{
    using Microsoft.AspNetCore.Http;
    public class UpdateSubCategoryDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;


        public string? Description { get; set; }

     
        public IFormFile? Image { get; set; }
}
}
