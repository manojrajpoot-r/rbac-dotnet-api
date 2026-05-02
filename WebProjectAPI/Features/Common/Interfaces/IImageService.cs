using Microsoft.AspNetCore.Http;

namespace WebProjectAPI.Features.Common.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);

        Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> files, string folderName);

        void DeleteImage(string imagePath);
    }
}