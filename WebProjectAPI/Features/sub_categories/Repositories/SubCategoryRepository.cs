using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.sub_categories.Interfaces;
using WebProjectAPI.Features.sub_categories.Models;

namespace WebProjectAPI.Features.sub_categories.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly AppDbContext _context;

        public SubCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubCategory>> GetAllAsync()
        {

            return await _context.SubCategories
                    .Include(x => x.Category)
                    .ToListAsync();
        }

        public async Task<SubCategory?> GetByIdAsync(int id)
        {
            return await _context.SubCategories.FindAsync(id);
        }

        public async Task<SubCategory> CreateAsync(SubCategory subCategory)
        {
            _context.SubCategories.Add(subCategory  );

            await _context.SaveChangesAsync();

            return subCategory;
        }

        public async Task<SubCategory> UpdateAsync(SubCategory subCategory)
        {
            _context.SubCategories.Update(subCategory   );

            await _context.SaveChangesAsync();

            return subCategory;
        }

        public async Task<bool> DeleteAsync(SubCategory subCategory)
        {
            _context.SubCategories.Remove(subCategory);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory == null)
                return false;
            subCategory.Status = !subCategory.Status;
            _context.SubCategories.Update(subCategory   );
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<SubCategory>> GetAllSubCategoriesAsync()
        {
            return await _context.SubCategories
                .Where(x => x.Status == true)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
