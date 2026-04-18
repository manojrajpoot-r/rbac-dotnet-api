using WebProjectAPI.Models;

namespace WebProjectAPI.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        List<Role> GetAll();
        Role GetById(int id);
        Role Add(Role role);
        Role Update(Role role);
        bool Delete(int id);
        bool ToggleStatus(int id);
    }
}
