using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Categories.Interfaces;
using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Services.Interfaces;


namespace WebProjectAPI.Features.Categories.Repositories
{
   
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public CategoryRepository(AppDbContext context,ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUser = currentUserService;
        }

        public async Task<ApiResponse<List<CategoryListDto>>> GetAllAsync(
           PaginationRequest request)
        {
            var query = _context.Categories.AsQueryable();

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
                .Select(x => new CategoryListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = x.Status
                })
                .ToListAsync();

            return new ApiResponse<List<CategoryListDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
         .FirstOrDefaultAsync(x =>
             x.Id == id &&
             x.TenantId == _currentUser.TenantId);
        }
        public async Task<Category> CreateAsync(Category category)
        {
            category.TenantId = _currentUser.TenantId.Value;

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return category;
        }
        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(x =>
                    x.Id == category.Id &&
                    x.TenantId == _currentUser.TenantId);

            if (existingCategory == null)
                throw new Exception("Category not found");

            existingCategory.Name = category.Name;
            existingCategory.Status = category.Status;

            await _context.SaveChangesAsync();

            return existingCategory;
        }

  

        public async Task<bool> DeleteAsync(Category category)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(x =>
                    x.Id == category.Id &&
                    x.TenantId == _currentUser.TenantId);

            if (existingCategory == null)
                return false;

            _context.Categories.Remove(existingCategory);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);

            if (category == null)
                return false;

            category.Status = !category.Status;

            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Where(x =>
                    x.Status &&
                    x.TenantId == _currentUser.TenantId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
