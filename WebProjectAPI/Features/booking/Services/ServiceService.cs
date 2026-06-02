using WebProjectAPI.Features.booking.DTOs;
using WebProjectAPI.Features.booking.Interfaces;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repository;
        private readonly IImageService _imageService;

        public ServiceService(
            IServiceRepository repository,IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }

        // LIST
        public async Task<ApiResponse<List<Service>>> GetAll(
            PaginationRequest request)
        {
            return await _repository.GetAll(request);
        }

        // GET BY ID
        public async Task<ApiResponse<Service>> GetById(
            int id)
        {
            return await _repository.GetById(id);
        }

        // ADD
        public async Task<ApiResponse<Service>> Add(CreateServiceDto model)
        {
            string imagePath = "";

            if (model.ImageUrl != null)
            {
                imagePath = await _imageService
                    .UploadImageAsync(model.ImageUrl, "services");
            }

            var service = new Service
            {
                ServiceName = model.ServiceName,
                Price = model.Price,
                DurationMinutes = model.DurationMinutes,
                Description = model.Description,
                ImageUrl = imagePath
            };

            return await _repository.Add(service);
        }
       

        // UPDATE
        public async Task<ApiResponse<Service>> Update(
            UpdateServiceDto model)
        {
            var service = new Service
            {
                Id = model.Id,
                ServiceName = model.ServiceName,
                Price = model.Price,
                DurationMinutes = model.DurationMinutes,
                Description = model.Description,
            };
            if (model.ImageUrl != null)
            {
                if (!string.IsNullOrEmpty(service.ImageUrl))
                {
                    _imageService.DeleteImage(service.ImageUrl);
                }

                service.ImageUrl =
                    await _imageService.UploadImageAsync(model.ImageUrl, "services");
            }


            return await _repository.Update(service);
        }

        // DELETE
        public async Task<ApiResponse<string>> Delete(
            int id)
        {
            return await _repository.Delete(id);
        }

        // STATUS
        public async Task<ApiResponse<string>> ChangeStatus(
            int id)
        {
            return await _repository.ChangeStatus(id);
        }

        // DROPDOWN
        public async Task<ApiResponse<List<Service>>> Dropdown()
        {
            var data = await _repository.Dropdown();

            return new ApiResponse<List<Service>>
            {
                Success = true,
                Message = "Dropdown fetched successfully",
                Data = data
            };
        }


    }
}