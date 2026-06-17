using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Repositories.Implementations
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public UserRepository(AppDbContext context,ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public List<User> GetAll(PaginationRequest request)
        {
            var query = _context.Users.AsQueryable();

            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x =>
                    x.TenantId == _currentUser.TenantId);
            }

            //  search
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.FullName.Contains(request.Search) || x.Email.Contains(request.Search));
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

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            var existing = _context.Users.Find(user.Id);
            if (existing == null) return null;

            existing.FullName = user.FullName ?? existing.FullName;
          

            _context.SaveChanges();
            return existing;
        }

        public bool Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public bool ToggleStatus(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            user.Status = user.Status==1?0:1;
            _context.SaveChanges();
            return true;
        }
    }
}


