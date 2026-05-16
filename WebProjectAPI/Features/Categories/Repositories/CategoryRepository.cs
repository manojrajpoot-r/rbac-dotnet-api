using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Categories.Interfaces;
using WebProjectAPI.Features.Categories.Models;


namespace WebProjectAPI.Features.Categories.Repositories
{
   
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Category> Data, int TotalRecords)>
    GetAllAsync(
        int pageNumber,
        int pageSize,
        string search)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(
                    x => x.Name.Contains(search)
                );
            }

            int totalRecords =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }


        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;
            category.Status = !category.Status;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<List<Category>>
     GetCategoriesAsync()
        {
            return await _context.Categories
                .Where(x => x.Status)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
