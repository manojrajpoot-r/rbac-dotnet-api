using WebProjectAPI.Features.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace WebProjectAPI.Features.Common.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            var folderPath = Path.Combine(_environment.WebRootPath, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"{folderName}/{fileName}";
        }

        public async Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> files, string folderName)
        {
            var images = new List<string>();

            foreach (var file in files)
            {
                var image = await UploadImageAsync(file, folderName);

                images.Add(image);
            }

            return images;
        }

        public void DeleteImage(string imagePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, imagePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}