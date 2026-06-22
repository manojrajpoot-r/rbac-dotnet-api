using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.brands.Interfaces;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Services.Interfaces;



namespace WebProjectAPI.Features.brands.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;
        private  readonly ICurrentUserService _currentUser;

        public BrandRepository(AppDbContext context,ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUser = currentUserService;
            
        }

        public async Task<ApiResponse<List<BrandListDto>>> GetAllAsync(
        PaginationRequest request)
        {
            var query = _context.Brands.AsQueryable();

            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x =>
                    x.TenantId == _currentUser.TenantId);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(request.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BrandListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug=x.Slug,
                    Image=x.Image,
                    IsFeatured=x.IsFeatured,
                    Status = x.Status
                })
                .ToListAsync();

            return new ApiResponse<List<BrandListDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands
            .FirstOrDefaultAsync(x =>
            x.Id == id &&
            x.TenantId == _currentUser.TenantId);
        }

        public async Task<Brand> CreateAsync(Brand brand)
        {
            brand.TenantId = _currentUser.TenantId.Value;
            _context.Brands.Add(brand);

            await _context.SaveChangesAsync();

            return brand;
        }

        public async Task<Brand> UpdateAsync(Brand brand)
        {
            var existingBrand = await _context.Brands
                .FirstOrDefaultAsync(x =>
                    x.Id == brand.Id &&
                    x.TenantId == _currentUser.TenantId);

            if (existingBrand == null)
                throw new Exception("Brand not found");

            existingBrand.Name = brand.Name;
            existingBrand.Status = brand.Status;

            await _context.SaveChangesAsync();

            return existingBrand;
        }

        public async Task<bool> DeleteAsync(Brand brand)
        {
            var existingBrand = await _context.Brands
                .FirstOrDefaultAsync(x =>
                    x.Id == brand.Id &&
                    x.TenantId == _currentUser.TenantId);

            if (existingBrand == null)
                return false;

            _context.Brands.Remove(existingBrand);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var brand = await _context.Brands
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);

            if (brand == null)
                return false;

            brand.Status = !brand.Status;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Brand>>
       GetAllBrandsAsync()
        {
            return await _context.Brands
         .Where(x =>
             x.Status &&
             x.TenantId == _currentUser.TenantId)
         .OrderBy(x => x.Name)
         .ToListAsync();
        }
    }
}