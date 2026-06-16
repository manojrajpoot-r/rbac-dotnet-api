using Microsoft.EntityFrameworkCore;
using System.Security;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;

namespace WebProjectAPI.Repositories.Implementations
{

        public class PermissionRepository : IPermissionRepository
        {
            private readonly AppDbContext _context;

            public PermissionRepository(AppDbContext context)
            {
                _context = context;
            }

        public List<Permission> GetAll(PaginationRequest request)
        {
            var query = _context.Permissions.AsQueryable();

            //  search
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.GroupName.Contains(request.Search));
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

             public Permission GetById(int id)
            {
                return _context.Permissions.FirstOrDefault(x => x.Id == id);
            }

            public Permission Add(Permission p)
            {
                _context.Permissions.Add(p);
                _context.SaveChanges();
                return p;
            }

            public Permission Update(Permission p)
            {
                var existing = _context.Permissions.Find(p.Id);
                if (existing == null) return null;

                existing.Name = p.Name ?? existing.Name;
                _context.SaveChanges();
                return existing;
            }

            public bool Delete(int id)
            {
                var p = _context.Permissions.Find(id);
                if (p == null) return false;

                _context.Permissions.Remove(p);
                _context.SaveChanges();
                return true;
            }

           
        }
    }


