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
        public async Task<ApiResponse<List<BookingResponseDto>>> GetAll(
        PaginationRequest request)
        {
            var data = await _context.Bookings
                .Include(x => x.BookingServiceItems)
                .ToListAsync();

            var result = data.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                UserId = b.UserId,
                BookingDate = b.BookingDate.ToString("yyyy-MM-dd"),
                BookingTime = b.BookingTime,
                TotalAmount = b.TotalAmount,
                BookingStatus = b.BookingStatus,
                PaymentStatus = b.PaymentStatus,

                Services = b.BookingServiceItems.Select(s => new BookingServiceItemDto
                {
                    ServiceId = s.ServiceId,
                    Price = s.Price
                }).ToList()
            }).ToList();

            return new ApiResponse<List<BookingResponseDto>>
            {
                Success = true,
                Data = result
            };
        }

        // GET BY ID
        public async Task<ApiResponse<BookingResponseDto>> GetById(int id)
        {
            var data = await _repository.GetById(id);

            if (data == null)
            {
                return new ApiResponse<BookingResponseDto>
                {
                    Success = false,
                    Message = "Not found"
                };
            }

            return new ApiResponse<BookingResponseDto>
            {
                Success = true,
                Data = new BookingResponseDto
                {
                    Id = data.Id,
                    UserId = data.UserId,
                    UserName = data.User?.Name, 
                    BookingDate = data.BookingDate.ToString("yyyy-MM-dd"),
                    BookingTime = data.BookingTime,
                    TotalAmount = data.TotalAmount,
                    BookingStatus = data.BookingStatus,
                    PaymentStatus = data.PaymentStatus,
                    Address       = data.Address,
                    Notes         = data.Notes,
                    PaymentMethod = data.PaymentMethod,

                    Services = data.BookingServiceItems.Select(x => new BookingServiceItemDto
                    {
                        ServiceId = x.ServiceId,
                        ServiceName = x.Service.ServiceName,
                        Price = x.Price
                    }).ToList()
                }
            };
        }

        // ADD
        public async Task<ApiResponse<BookingResponseDto>> Add(CreateBookingDto model)
        {
            var services = await _context.Services
                .Where(x => model.ServiceIds.Contains(x.Id))
                .ToListAsync();

            decimal total = services.Sum(x => x.Price);

            var booking = new Booking
            {
                UserId = model.UserId,
                BookingDate = model.BookingDate,
                BookingTime = model.BookingTime.ToString(),
                TotalAmount = total,
                BookingStatus = "Pending",
                PaymentStatus = "Pending",
                PaymentMethod = model.PaymentMethod,
                Notes = model.Notes,
                Address = model.Address,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now,

                BookingServiceItems = services.Select(x => new BookingServiceItem
                {
                    ServiceId = x.Id,
                    Price = x.Price
                }).ToList()
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(); // IMPORTANT

            var result = new BookingResponseDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                BookingDate = booking.BookingDate.ToString("yyyy-MM-dd"),
                BookingTime = booking.BookingTime,
                TotalAmount = booking.TotalAmount,
                BookingStatus = booking.BookingStatus,
                PaymentStatus = booking.PaymentStatus,

                Services = booking.BookingServiceItems.Select(x => new BookingServiceItemDto
                {
                    ServiceId = x.ServiceId,
                    Price = x.Price
                }).ToList()
            };

            return new ApiResponse<BookingResponseDto>
            {
                Success = true,
                Data = result
            };
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

            bookingData.BookingTime = model.BookingTime.ToString();
        
            bookingData.TotalAmount = total;

            bookingData.PaymentMethod = model.PaymentMethod;

            bookingData.Notes = model.Notes;

            bookingData.Address = model.Address;

            bookingData.UpdatedAt = DateTime.Now;

            // REMOVE OLD SERVICES
            _context.BookingServiceItems.RemoveRange(
      bookingData.BookingServiceItems);

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

        public async Task<ApiResponse<List<BookingResponseDto>>> GetByUser(int userId)
        {
            

            var data = await _repository.GetByUser(userId);

            if (data == null || !data.Any())
            {
                return new ApiResponse<List<BookingResponseDto>>
                {
                    Success = true,
                    Data = new List<BookingResponseDto>()
                };
            }

            var result = data.Select(x => new BookingResponseDto
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.User?.Name,
                BookingStatus = x.BookingStatus,
                PaymentStatus = x.PaymentStatus,
                PaymentMethod = x.PaymentMethod,
                TotalAmount = x.TotalAmount,
                BookingDate = x.BookingDate.ToString("yyyy-MM-dd"),
                BookingTime = x.BookingTime,
                Address     = x.Address,
                Notes        = x.Notes,


                Services = x.BookingServiceItems?
                    .Select(s => new BookingServiceItemDto
                    {
                        ServiceId = s.ServiceId,
                        Price = s.Price,
                        ServiceName = s.Service?.ServiceName
                    })
                    .ToList() ?? new List<BookingServiceItemDto>()
            }).ToList();

            return new ApiResponse<List<BookingResponseDto>>
            {
                Success = true,
                Data = result
            };
        }
    }
}