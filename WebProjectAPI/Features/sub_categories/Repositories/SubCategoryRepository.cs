using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.sub_categories.DTOs;
using WebProjectAPI.Features.sub_categories.Interfaces;
using WebProjectAPI.Features.sub_categories.Models;
using WebProjectAPI.Services.Interfaces;


namespace WebProjectAPI.Features.sub_categories.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public SubCategoryRepository(AppDbContext context,ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUser = currentUserService;
        }

        public async Task<ApiResponse<List<SubCategoryListDto>>> GetAllAsync(
         PaginationRequest request)
        {
            var query = _context.SubCategories
                .Include(x => x.Category)
                .AsQueryable();

            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x => x.TenantId == _currentUser.TenantId);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x => x.Name.Contains(request.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new SubCategoryListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = x.Status,
                    CategoryId=x.CategoryId,
                    CategoryName = x.Category.Name,
                    Description=x.Description,
                    Image=x.Image
                })
                .ToListAsync();

            return new ApiResponse<List<SubCategoryListDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }


        public async Task<SubCategory?> GetByIdAsync(int id)
        {
            return await _context.SubCategories
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);
        }

        public async Task<SubCategory> CreateAsync(SubCategory subCategory)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(x => x.Id == subCategory.CategoryId);

            if (!categoryExists)
                throw new Exception("Invalid CategoryId");

            subCategory.TenantId = _currentUser.TenantId.Value;

            _context.SubCategories.Add(subCategory);
            await _context.SaveChangesAsync();

            return subCategory;
        }

        public async Task<SubCategory> UpdateAsync(
      SubCategory subCategory)
        {
            var existingSubCategory =
                await _context.SubCategories
                .FirstOrDefaultAsync(x =>
                    x.Id == subCategory.Id &&
                    x.TenantId == _currentUser.TenantId);

            if (existingSubCategory == null)
                throw new Exception("Sub Category not found");

            existingSubCategory.Name =
                subCategory.Name;

            existingSubCategory.CategoryId =
                subCategory.CategoryId;

            existingSubCategory.Status =
                subCategory.Status;

            await _context.SaveChangesAsync();

            return existingSubCategory;
        }

        public async Task<bool> DeleteAsync(
            SubCategory subCategory)
        {
            var existingSubCategory =
                await _context.SubCategories
                .FirstOrDefaultAsync(x =>
                    x.Id == subCategory.Id &&
                    x.TenantId == _currentUser.TenantId);

            if (existingSubCategory == null)
                return false;

            _context.SubCategories
                .Remove(existingSubCategory);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var subCategory =
                await _context.SubCategories
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);

            if (subCategory == null)
                return false;

            subCategory.Status =
                !subCategory.Status;

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<SubCategory>>
       GetAllSubCategoriesAsync()
        {
            return await _context.SubCategories
                .Where(x =>
                    x.Status &&
                    x.TenantId == _currentUser.TenantId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
