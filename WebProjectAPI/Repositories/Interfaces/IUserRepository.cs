using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Models;

namespace WebProjectAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll(PaginationRequest request);
        User GetById(int id);
        User Add(User user);
        User Update(User user);
        bool Delete(int id);
        bool ToggleStatus(int id);
    }
}
