using WebProjectAPI.Models;

namespace WebProjectAPI.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        List<Role> GetAll(int pageNumber, int pageSize, string search, out int totalRecords);
        Role GetById(int id);
        Role Add(Role role);
        Role Update(Role role);
        bool Delete(int id);
        bool ToggleStatus(int id);
    }
}
