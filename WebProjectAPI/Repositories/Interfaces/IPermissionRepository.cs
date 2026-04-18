using WebProjectAPI.Models;

namespace WebProjectAPI.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        List<Permission> GetAll();
        Permission GetById(int id);
        Permission Add(Permission p);
        Permission Update(Permission p);
        bool Delete(int id);
       
    }
}
