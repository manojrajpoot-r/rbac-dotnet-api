using WebProjectAPI.Data;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;

namespace WebProjectAPI.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Role> GetAll(int pageNumber, int pageSize, string search, out int totalRecords)
        {
            var query = _context.Roles.AsQueryable();

            //  search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            // 📊 total count
            totalRecords = query.Count();

            // 📄 pagination
            return query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public Role GetById(int id)
        {
            return _context.Roles.FirstOrDefault(x => x.Id == id);
        }

        public Role Add(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }

        public Role Update(Role role)
        {
            var existing = _context.Roles.Find(role.Id);
            if (existing == null) return null;

            existing.Name = role.Name;
            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            _context.SaveChanges();
            return true;
        }

        public bool ToggleStatus(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null) return false;

            role.Status = role.Status == 1 ? 0 : 1;
            _context.SaveChanges();
            return true;
        }
    }
}
