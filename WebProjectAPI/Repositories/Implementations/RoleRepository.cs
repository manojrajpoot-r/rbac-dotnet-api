using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public RoleRepository(AppDbContext context,ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public List<Role> GetAll(PaginationRequest request)
        {
            var query = _context.Roles.AsQueryable();
            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x =>
                    x.TenantId == _currentUser.TenantId);
            }
            //  search
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.Contains(request.Search));
            }

            // 📊 total count
          int totalRecords = query.Count();

            // 📄 pagination
            return query
                .OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
        }


        public Role GetById(int id)
        {
            return _context.Roles.FirstOrDefault(x => x.Id == id);
        }

        public Role Add(Role role)
        {
            if (!_currentUser.TenantId.HasValue)
                throw new Exception("TenantId not found in token");

            role.TenantId = _currentUser.TenantId.Value;
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
