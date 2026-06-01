using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.booking.Interfaces;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        // LIST
        public async Task<ApiResponse<List<Booking>>> GetAll(
            PaginationRequest request)
        {
            var query = _context.Bookings
                .Where(x => !x.IsDeleted)
                .Include(x => x.BookingServiceItems)
                .ThenInclude(x => x.Service)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new ApiResponse<List<Booking>>
            {
                Success = true,
                Message = "Booking list fetched successfully",
                Data = data,
                TotalRecords = totalRecords
            };
        }

        // GET BY ID
        public async Task<Booking?> GetById(int id)
        {
            return await _context.Bookings
                .Include(x => x.BookingServiceItems)
                    .ThenInclude(x => x.Service)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        // ADD
        public async Task<ApiResponse<Booking>> Add(
            Booking model)
        {
            await _context.Bookings.AddAsync(model);

            await _context.SaveChangesAsync();

            return new ApiResponse<Booking>
            {
                Success = true,
                Message = "Booking added successfully",
                Data = model
            };
        }

        // UPDATE
        public async Task<ApiResponse<Booking>> Update(
            Booking model)
        {
            var booking = await _context.Bookings
                .Include(x => x.BookingServiceItems)
                .FirstOrDefaultAsync(x =>
                    x.Id == model.Id);

            if (booking == null)
            {
                return new ApiResponse<Booking>
                {
                    Success = false,
                    Message = "Booking not found"
                };
            }

            booking.UserId = model.UserId;

            booking.BookingDate = model.BookingDate;

            booking.BookingTime = model.BookingTime;

            booking.TotalAmount = model.TotalAmount;

            booking.PaymentMethod = model.PaymentMethod;

            booking.PaymentStatus = model.PaymentStatus;

            booking.BookingStatus = model.BookingStatus;

            booking.Notes = model.Notes;

            booking.Address = model.Address;

            booking.UpdatedAt = DateTime.Now;

            // REMOVE OLD SERVICES
            _context.BookingServiceItems.RemoveRange(
                booking.BookingServiceItems);

            // ADD NEW SERVICES
            booking.BookingServiceItems =
                model.BookingServiceItems;

            await _context.SaveChangesAsync();

            return new ApiResponse<Booking>
            {
                Success = true,
                Message = "Booking updated successfully",
                Data = booking
            };
        }

        // DELETE
        public async Task<ApiResponse<string>> Delete(
            int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.Id == id);

            if (booking == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Booking not found"
                };
            }

            booking.IsDeleted = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Booking deleted successfully"
            };
        }

        // STATUS
        public async Task<ApiResponse<string>> ChangeStatus(
            int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.Id == id);

            if (booking == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Booking not found"
                };
            }

            booking.IsActive = !booking.IsActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Status changed successfully"
            };
        }

        public async Task<List<Booking>> GetByUser(int userId)
        {
            return await _context.Bookings
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}