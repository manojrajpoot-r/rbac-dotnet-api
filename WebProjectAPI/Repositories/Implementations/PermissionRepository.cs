using WebProjectAPI.Data;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace WebProjectAPI.Repositories.Implementations
{

        public class PermissionRepository : IPermissionRepository
        {
            private readonly AppDbContext _context;

            public PermissionRepository(AppDbContext context)
            {
                _context = context;
            }

            public List<Permission> GetAll()
            {
                return _context.Permissions.ToList();
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


