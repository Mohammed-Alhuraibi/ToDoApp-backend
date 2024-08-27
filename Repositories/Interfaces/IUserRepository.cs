using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task SaveChangesAsync();
        Task<User> GetUserByEmailAsync(string email);
    }
}
