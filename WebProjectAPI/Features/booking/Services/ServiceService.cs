using WebProjectAPI.Features.booking.DTOs;
using WebProjectAPI.Features.booking.Interfaces;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repository;

        public ServiceService(
            IServiceRepository repository)
        {
            _repository = repository;
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
        public async Task<ApiResponse<Service>> Add(
            CreateServiceDto model)
        {
            var service = new Service
            {
                ServiceName = model.ServiceName,
                Price = model.Price,
                DurationMinutes =
                    model.DurationMinutes
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
                DurationMinutes =
                    model.DurationMinutes
            };

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