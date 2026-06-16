using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Models;

namespace WebProjectAPI.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        List<Role> GetAll(PaginationRequest request);
        Role GetById(int id);
        Role Add(Role role);
        Role Update(Role role);
        bool Delete(int id);
        bool ToggleStatus(int id);
    }
}
