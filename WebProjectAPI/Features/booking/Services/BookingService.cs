using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.booking.DTOs;
using WebProjectAPI.Features.booking.Interfaces;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly AppDbContext _context;

        public BookingService(
            IBookingRepository repository,
            AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // LIST
        public async Task<ApiResponse<List<Booking>>> GetAll(
            PaginationRequest request)
        {
            return await _repository.GetAll(request);
        }

        // GET BY ID
        public async Task<ApiResponse<Booking>> GetById(
            int id)
        {
            return await _repository.GetById(id);
        }

        // ADD
        public async Task<ApiResponse<Booking>> Add(
            CreateBookingDto model)
        {
            var services = await _context.Services
                .Where(x => model.ServiceIds.Contains(x.Id))
                .ToListAsync();

            decimal total = services.Sum(x => x.Price);

            var booking = new Booking
            {
                UserId = model.UserId,

                BookingDate = model.BookingDate,

                BookingTime = model.BookingTime,

                TotalAmount = total,

                BookingStatus = "Pending",

                PaymentStatus = "Pending",

                PaymentMethod = model.PaymentMethod,

                Notes = model.Notes,

                Address = model.Address,

                IsActive = true,

                IsDeleted = false,

                CreatedAt = DateTime.Now,

                BookingServiceItems = services
                    .Select(x => new BookingServiceItem
                    {
                        ServiceId = x.Id,
                        Price = x.Price
                    }).ToList()
            };

            return await _repository.Add(booking);
        }

        // UPDATE
        public async Task<ApiResponse<Booking>> Update(
            UpdateBookingDto model)
        {
            var bookingData = await _context.Bookings
                .Include(x => x.BookingServiceItems)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (bookingData == null)
            {
                return new ApiResponse<Booking>
                {
                    Success = false,
                    Message = "Booking not found"
                };
            }

            var services = await _context.Services
                .Where(x => model.ServiceIds.Contains(x.Id))
                .ToListAsync();

            decimal total = services.Sum(x => x.Price);

            bookingData.UserId = model.UserId;

            bookingData.BookingDate = model.BookingDate;

            bookingData.BookingTime = model.BookingTime;

            bookingData.TotalAmount = total;

            bookingData.PaymentMethod = model.PaymentMethod;

            bookingData.Notes = model.Notes;

            bookingData.Address = model.Address;

            bookingData.UpdatedAt = DateTime.Now;

            // REMOVE OLD SERVICES
            bookingData.BookingServiceItems.Clear();

            // ADD NEW SERVICES
            bookingData.BookingServiceItems = services
                .Select(x => new BookingServiceItem
                {
                    ServiceId = x.Id,
                    Price = x.Price
                }).ToList();

            return await _repository.Update(bookingData);
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
    }
}