using WebProjectAPI.Models;

namespace WebProjectAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll(int pageNumber, int pageSize, string search, out int totalRecords);
        User GetById(int id);
        User Add(User user);
        User Update(User user);
        bool Delete(int id);
        bool ToggleStatus(int id);
    }
}
