using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;

namespace WebProjectAPI.Repositories.Implementations
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll(int pageNumber, int pageSize, string search, out int totalRecords)
        {
            var query = _context.Users.AsQueryable();

            //  search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search) || x.Email.Contains(search));
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

            existing.Name = user.Name ?? existing.Name;
          

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


